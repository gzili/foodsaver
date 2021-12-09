import { Button, Grid, useDisclosure } from '@chakra-ui/react';

import { OfferDeletePrompt, OfferReservations } from './components';

export function GiverActions() {
  const { isOpen: isDeletePromptOpen, onOpen, onClose: onDeletePromptClose } = useDisclosure();

  return (
    <>
      <OfferDeletePrompt isOpen={isDeletePromptOpen} onClose={onDeletePromptClose} />
      <Grid templateColumns="2fr 1fr" gap={2}>
        <OfferReservations />
        <Button colorScheme="red" onClick={onOpen}>Delete</Button>
      </Grid>
    </>
  );
}