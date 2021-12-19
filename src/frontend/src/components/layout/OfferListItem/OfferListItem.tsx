import { Box, Flex, Avatar, HStack } from '@chakra-ui/react';
import { faMapMarkerAlt } from '@fortawesome/free-solid-svg-icons';
import { isPast, parseJSON, formatDistanceToNowStrict } from 'date-fns';
import { Link as RouterLink } from 'react-router-dom';

import { FaIcon } from 'components';
import { IOfferDto } from 'dto/offer';
import { absPath } from 'helpers';

interface IOffersListItem {
  item: IOfferDto,
}

export default function OfferListItem(props: IOffersListItem) {
  const { item } = props;

  const isExpired = isPast(parseJSON(item.expiresAt));
  const toNow = formatDistanceToNowStrict(parseJSON(item.createdAt), { addSuffix: true });

  return (
    <Box
      as={RouterLink}
      to={'/app/offers/' + item.id}
      pos="relative"
      w="100%"
      h="180px"
      overflow="hidden"
      bg={`url('${absPath(item.food.imagePath, true)}')`}
      bgPos="center"
      bgSize="cover"
      borderRadius="xl"
      opacity={isExpired ? 0.5 : undefined}
      pointerEvents={isExpired ? 'none' : undefined}
    >
      <Flex direction="column" justify="space-between" pos="absolute" inset={0} p={4} bg="rgba(0, 0, 0, 0.4)" color="white">
        <Box>
          <Flex align="center">
            <Avatar size="sm" name={item.giver.username} src={absPath(item.giver.avatarPath)} mr={2} />
            {item.giver.username}
          </Flex>
        </Box>
        <Box>
          <Box fontSize="xs" color="rgba(255, 255, 255, 0.8)">{[item.quantity, item.food.unit, '•', toNow].join(' ')}</Box>
          <Box fontWeight="bold" fontSize="lg">{item.food.name}</Box>
          <HStack spacing={2} fontSize="sm">
            <Box as={FaIcon} icon={faMapMarkerAlt} />
            <Box>{[item.giver.address.street, item.giver.address.city].join(', ')}</Box>
          </HStack>
        </Box>
      </Flex>
    </Box>
  );
}