import { Box, Flex, HStack, Square } from '@chakra-ui/react';
import { faCheck, FaIcon } from 'components';
import { useOffer } from 'routes/app/offers/[id]/contexts/OfferContext';
import { ICreatorReservation } from '../types';

export function ReservationCompleted({ reservation }: { reservation: ICreatorReservation }) {
  const offer = useOffer();

  return (
    <Flex justifyContent="center">
      <HStack>
        <Square bg="green.50" borderRadius="md" color="green.500" size={10}>
          <FaIcon icon={faCheck} />
        </Square>
        <Box>You received {reservation.quantity} {offer.food.unit}.</Box>
      </HStack>
    </Flex>
  );
}