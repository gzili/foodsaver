import { Button, Grid, useDisclosure } from '@chakra-ui/react';
import { FaIcon, faTrashAlt } from 'components';

import { OfferDeletePrompt, OfferReservations } from './components';

export function GiverActions() {
  const { isOpen: isDeletePromptOpen, onOpen, onClose: onDeletePromptClose } = useDisclosure();

  return (
    <>
      <OfferDeletePrompt isOpen={isDeletePromptOpen} onClose={onDeletePromptClose} />
      <Grid templateColumns="2fr 1fr" gap={2}>
        <OfferReservations />
        <Button colorScheme="red" leftIcon={<FaIcon icon={faTrashAlt} />} onClick={onOpen}>Delete</Button>
      </Grid>
    </>
  );
}