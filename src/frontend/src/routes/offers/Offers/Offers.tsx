import { Avatar, Box, Flex, Heading, VStack } from '@chakra-ui/react';
import { useQuery } from 'react-query';
import { Link as RouterLink } from 'react-router-dom';
import { formatDistanceToNowStrict, parseJSON } from 'date-fns';

import { FaIcon, faMapMarkerAlt, faUser } from 'components/core';

import { OfferItem } from './data/offers';

interface IOffer {
  item: OfferItem,
}

function OffersListItem(props: IOffer) {
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

async function fetchOffers() {
  const res = await fetch('api/offers');

  if (!res.ok) {
    throw new Error(`Error getting offers: server responded with status ${res.status} ${res.statusText}`)
  }

  const offers: OfferItem[] = await res.json();

  return offers;
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