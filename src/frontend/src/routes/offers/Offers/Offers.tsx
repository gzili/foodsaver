import { Avatar, Box, Flex, Heading, VStack } from '@chakra-ui/react';

import { FaIcon, faMapMarkerAlt } from 'components/core';

import { OfferItem, offers } from './data/offers';

interface IOffer {
  item: OfferItem,
}

function Offer(props: IOffer) {
  const { item } = props;

  return (
    <Box pos="relative" w="100%" h="180px" overflow="hidden" borderRadius="xl" bg={`url('${item.food.photo}')`} bgPos="center" bgSize="cover">
      <Flex direction="column" justify="space-between" pos="absolute" inset={0} p={4} bg="rgba(0, 0, 0, 0.4)" color="white">
        <Box>
          <Flex align="center">
            <Avatar size="sm" name={item.user.name} src={item.user.avatar} mr={2} />
            {item.user.name}
          </Flex>
        </Box>
        <Box>
          <Box fontSize="xs" color="rgba(255, 255, 255, 0.8)">{item.dateCreated}</Box>
          <Box fontWeight="bold" fontSize="lg">{item.food.name}</Box>
          <Flex fontSize="sm" align="center">
            <Box as={FaIcon} icon={faMapMarkerAlt} mr={2} />
            {[item.location.street, item.location.city].join(', ')}
          </Flex>
        </Box>
      </Flex>
    </Box>
  );
}

export default function Offers() {
  return (
    <Box p={4}>
      <Heading as="h1" mb={2}>Offers</Heading>
      <VStack spacing={2}>
        {offers.map(item => (
          <Offer key={item.id} item={item} />
        ))}
      </VStack>
    </Box>
  );
}