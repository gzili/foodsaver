import { Link as RouterLink } from 'react-router-dom';
import { useQuery } from 'react-query';
import { formatDistanceToNowStrict, parseJSON } from 'date-fns';
import { Avatar, Box, Flex, Heading, VStack } from '@chakra-ui/react';

import { FaIcon, faMapMarkerAlt, faUser } from 'components/core';

import { IOfferDto } from 'dto/offer';

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
      bg={`url('${item.food.imagePath}')`}
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
  const res = await fetch('api/offers');

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
      <VStack spacing={2}>
        {data.map(offer => (
          <OffersListItem key={offer.id} item={offer} />
        ))}
      </VStack>
    );
  } else {
    return null;
  }
}

export default function Offers() {
  return (
    <Box p={4}>
      <Heading as="h1" mb={2}>Offers</Heading>
      <OffersList />
    </Box>
  );
}