import { Link as RouterLink } from 'react-router-dom';
import { useInfiniteQuery, useQuery } from 'react-query';
import { compareDesc, formatDistanceToNowStrict, parseJSON } from 'date-fns';
import { Avatar, Box, Button, Flex, Heading, IconButton, useDisclosure, VStack } from '@chakra-ui/react';
import { AddIcon } from '@chakra-ui/icons';
import { useAuth } from 'contexts/authContext';

import { FaIcon, faMapMarkerAlt } from 'components/core';
import { LoadingOverlay } from 'components/layout';
import { CreateOfferDrawer } from './components/CreateOfferDrawer';

import { IOfferDto } from 'dto/offer';
import { UserType } from 'contexts/authContext';
import { useHub } from 'contexts/hubContext';
import { Fragment, useCallback, useEffect } from 'react';
import api from 'contexts/apiContext';

interface IOffersListItem {
  item: IOfferDto,
}

function OffersListItem(props: IOffersListItem) {
  const { item } = props;

  const toNow = formatDistanceToNowStrict(parseJSON(item.createdAt), { addSuffix: true });

  return (
    <Box
      as={RouterLink}
      to={`/offers/${item.id}`}
      pos="relative"
      w="100%"
      h="180px"
      overflow="hidden"
      bg={`url('${item.food.imagePath.replaceAll('\\', '/')}')`}
      bgPos="center"
      bgSize="cover"
      borderRadius="xl">
      <Flex direction="column" justify="space-between" pos="absolute" inset={0} p={4} bg="rgba(0, 0, 0, 0.4)" color="white">
        <Box>
          <Flex align="center">
            <Avatar size="sm" name={item.giver.username} src={item.giver.avatarPath ?? undefined} mr={2} />
            {item.giver.username}
          </Flex>
        </Box>
        <Box>
          <Box fontSize="xs" color="rgba(255, 255, 255, 0.8)">{[item.quantity, item.food.unit, '•', toNow].join(' ')}</Box>
          <Box fontWeight="bold" fontSize="lg">{item.food.name}</Box>
          <Flex fontSize="sm" align="center">
            <Box as={FaIcon} icon={faMapMarkerAlt} mr={2} />
            {[item.giver.address.street, item.giver.address.city].join(', ')}
          </Flex>
        </Box>
      </Flex>
    </Box>
  );
}

interface IPaginated<T> {
  data: T[],
  hasNextPage: boolean,
}

const fetchOffers = ({ pageParam = 0 }) => {
  return api.get('offers?limit=3&page=' + pageParam).json<IPaginated<IOfferDto>>();
}

function OffersList() {
  const { isLoading, isError, data, error, refetch, fetchNextPage, hasNextPage, isFetchingNextPage } = useInfiniteQuery('offers', fetchOffers, {
    getNextPageParam: (lastPage, allPages) => lastPage.hasNextPage ? allPages.length : undefined,
  });

  const { connection } = useHub();

  const handleOffersChange = useCallback(() => refetch(), [refetch]);

  useEffect(() => {
    connection.on("OffersChanged", handleOffersChange);

    return () => connection.off("OffersChanged", handleOffersChange);
  }, [handleOffersChange, connection]);

  if (isLoading) {
    return <LoadingOverlay message="Loading offers" />
  }

  if (isError) {
    return <Box>{(error as Error).message}</Box>;
  }

  if (data) {
    return (
      <>
        <VStack spacing={2}>
          {data.pages.map((page, i) => (
            <Fragment key={i}>
              {page.data.map(offer => (
                <OffersListItem key={offer.id} item={offer} />
              ))}
            </Fragment>
          ))}
        </VStack>
        {hasNextPage && (
          <Box p={2} textAlign="center">
            <Button isLoading={isFetchingNextPage} onClick={() => fetchNextPage()}>Load more</Button>
          </Box>
        )}
      </>
    );
  }

  return null;
}

export default function Offers() {
  const { user } = useAuth();
  const { isOpen, onOpen, onClose } = useDisclosure();

  return (
    <Box mt={10} pt={2} px={4}>
      <Heading as="h1" mb={2}>Offers</Heading>
      <OffersList />
      {(user && user.userType !== UserType.Charity) && (
        <Box pb="80px">
          <IconButton
            icon={<AddIcon />}
            aria-label="Create offer"
            onClick={onOpen}
            colorScheme="brand"
            pos="fixed"
            right={6}
            bottom={6}
            borderRadius="full"
          />
          <CreateOfferDrawer isOpen={isOpen} onClose={onClose} />
        </Box>
      )}
    </Box>
  );
}