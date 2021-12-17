import {
  AlertDialog, AlertDialogBody, AlertDialogContent, AlertDialogFooter, AlertDialogHeader, AlertDialogOverlay,
  Button,
  HStack,
  useDisclosure
} from '@chakra-ui/react';
import { useRef } from 'react';
import { useMutation, useQueryClient } from 'react-query';

import { B } from 'components';
import api from 'contexts/apiContext';

import { useOffer } from 'routes/app/offers/[id]/contexts/OfferContext';
import type { IReservationPrompt, ReservationDto } from '../../../types';

function ReservationDeletePrompt(props: IReservationPrompt) {
  const { isOpen, onClose, quantity } = props;

  const queryClient = useQueryClient();

  const offer = useOffer();

  const cancelButtonRef = useRef<HTMLButtonElement>(null);

  const { mutate, isLoading } = useMutation({
    mutationFn: () => api.delete(`offers/${offer.id}/reservation`),
    onSuccess: () => {
      queryClient.setQueryData(['reservation', offer.id], undefined);
      onClose();
    },
    onError: err => console.log(err),
  });

  const handleReserve = () => {
    mutate();
  };

  return (
    <AlertDialog isOpen={isOpen} onClose={onClose} leastDestructiveRef={cancelButtonRef} isCentered size="xs">
      <AlertDialogOverlay />
      <AlertDialogContent>
        <AlertDialogHeader>Cancel reservation</AlertDialogHeader>
        <AlertDialogBody>Do you want to cancel your reservation for <B>{quantity} {offer?.food.unit}</B> of <B>{offer?.food.name}</B>?</AlertDialogBody>
        <AlertDialogFooter>
          <HStack spacing={2}>
            <Button onClick={onClose} isDisabled={isLoading}>Cancel</Button>
            <Button colorScheme="red" onClick={handleReserve} isLoading={isLoading}>Confirm</Button>
          </HStack>
        </AlertDialogFooter>
      </AlertDialogContent>
    </AlertDialog> 
  );
}

export function ReservationDelete({ reservation }: { reservation: ReservationDto }) {
  const offer = useOffer();

  const { isOpen: isPromptOpen, onOpen, onClose } = useDisclosure();

  return (
    <>
      <ReservationDeletePrompt isOpen={isPromptOpen} onClose={onClose} quantity={reservation.quantity} />
      <Button colorScheme="red" onClick={onOpen} isDisabled={reservation.completedAt !== null}>Cancel ({reservation.quantity} {offer.food.unit})</Button>
    </>
  );
}