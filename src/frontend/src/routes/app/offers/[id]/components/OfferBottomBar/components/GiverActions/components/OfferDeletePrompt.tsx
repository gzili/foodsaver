import {
  AlertDialog, AlertDialogBody, AlertDialogContent, AlertDialogFooter, AlertDialogHeader, AlertDialogOverlay, Button, HStack
} from '@chakra-ui/react';
import { useCallback, useRef } from 'react';
import { useMutation } from 'react-query';

import api from 'contexts/apiContext';
import { useOffer } from 'routes/app/offers/[id]/contexts/OfferContext';

interface OfferDeletePromptProps {
  isOpen: boolean,
  onClose: () => void,
}

export function OfferDeletePrompt(props: OfferDeletePromptProps) {
  const { isOpen, onClose } = props;

  const offer = useOffer();

  const cancelButtonRef = useRef<HTMLButtonElement>(null);

  const { mutate, isLoading } = useMutation(() => api.delete(`offers/${offer.id}`), {
    onError: err => console.log(err),
  });

  const handleDelete = useCallback(() => mutate(), [mutate]);

  return (
    <AlertDialog
      isOpen={isOpen}
      onClose={onClose}
      leastDestructiveRef={cancelButtonRef}
      size="xs"
      isCentered
    >
      <AlertDialogOverlay />
      <AlertDialogContent>
        <AlertDialogHeader>Delete offer</AlertDialogHeader>
        <AlertDialogBody>Do you want to permanently delete the offer cancelling all associated reservations?</AlertDialogBody>
        <AlertDialogFooter>
          <HStack spacing={2}>
            <Button onClick={onClose}>Cancel</Button>
            <Button colorScheme="red" onClick={handleDelete} isLoading={isLoading}>Delete</Button>
          </HStack>
        </AlertDialogFooter>
      </AlertDialogContent>
    </AlertDialog>
  );
}