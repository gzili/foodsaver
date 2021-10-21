import { Link as RouterLink } from 'react-router-dom';
import { useQuery } from 'react-query';
import { compareDesc, formatDistanceToNowStrict, parseJSON } from 'date-fns';
import { Avatar, Box, Flex, Heading, IconButton, useDisclosure, VStack } from '@chakra-ui/react';
import { AddIcon } from '@chakra-ui/icons';
import { useAuth } from 'contexts/auth.context';

import { FaIcon, faMapMarkerAlt, faUser } from 'components/core';
import { CreateOfferDrawer } from './components/CreateOfferDrawer';

import { IOfferDto } from 'dto/offer';
import { UserType } from 'contexts/auth.context';

interface IOffersListItem {
  item: IOfferDto,
}

function OffersListItem(props: IOffersListItem) {
  const { item } = props;

  const toNow = formatDistanceToNowStrict(parseJSON(item.creationDate), { addSuffix: true });

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
            <Avatar size="sm" name={item.giver.name} /*src={item.user.avatar}*/ icon={<FaIcon icon={faUser} />} mr={2} />
            {item.giver.name}
          </Flex>
        </Box>
        <Box>
          <Box fontSize="xs" color="rgba(255, 255, 255, 0.8)">{[item.quantity, item.food.unit, '•', toNow].join(' ')}</Box>
          <Box fontWeight="bold" fontSize="lg">{item.food.name}</Box>
          <Flex fontSize="sm" align="center">
            <Box as={FaIcon} icon={faMapMarkerAlt} mr={2} />
            {[item.giver.address.streetAddress, item.giver.address.city].join(', ')}
          </Flex>
        </Box>
      </Flex>
    </Box>
  );
}

async function fetchOffers(): Promise<IOfferDto[]> {
  const res = await fetch('api/offers?showExpired');

  if (!res.ok) {
    throw new Error(`Unable to fetch offers: server responded with status ${res.status} ${res.statusText}`)
  }

  return res.json();
}

function OffersList() {
  const { isLoading, isError, data, error } = useQuery('offers', fetchOffers);

  if (isLoading) {
    return <Box>{"Loading offers..."}</Box>;
  }

  if (isError) {
    return <Box>{(error as Error).message}</Box>;
  }

  if (data) {
    return (
      <VStack spacing={2} pb="80px">
        {data.sort((a, b) => compareDesc(parseJSON(a.creationDate), parseJSON(b.creationDate))).map(offer => (
          <OffersListItem key={offer.id} item={offer} />
        ))}
      </VStack>
    );
  } else {
    return null;
  }
}

export default function Offers() {
  const { user } = useAuth();
  const { isOpen, onOpen, onClose } = useDisclosure();

  return (
    <Box mt={10} pt={2} px={4}>
      <Heading as="h1" mb={2}>Offers</Heading>
      <OffersList />
      {(user && user.userType !== UserType.Nonprofit) && (
        <>
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
        </>
      )}
    </Box>
  );
}