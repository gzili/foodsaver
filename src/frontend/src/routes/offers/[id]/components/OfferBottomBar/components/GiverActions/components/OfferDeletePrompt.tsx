import { useCallback, useRef } from 'react';
import { AlertDialog, AlertDialogOverlay, AlertDialogContent, AlertDialogHeader, AlertDialogBody, AlertDialogFooter, HStack, Button } from '@chakra-ui/react';
import { useMutation } from 'react-query';
import { useOffer } from 'routes/offers/[id]/contexts/OfferContext';
import api from 'contexts/api.context';

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