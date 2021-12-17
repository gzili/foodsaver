import { Box, Button, Modal, ModalBody, ModalCloseButton, ModalContent, ModalHeader, ModalOverlay, useDisclosure } from '@chakra-ui/react';
import { ICreatorReservation } from '../types';

interface IReservationPinDialog {
  isOpen: boolean,
  onClose: () => void,
  reservation: ICreatorReservation,
}

function ReservationPinDialog(props: IReservationPinDialog) {
  const {
    isOpen,
    onClose,
    reservation,
  } = props;

  return (
    <Modal isOpen={isOpen} onClose={onClose} size="xs" isCentered>
      <ModalOverlay />
      <ModalContent>
        <ModalHeader>Reservation PIN</ModalHeader>
        <ModalCloseButton />
        <ModalBody pt={0} pb={6}>
          <Box textAlign="center">PIN code for this reservation:</Box>
          <Box fontSize="4xl" fontWeight="bold" textAlign="center">{reservation.pin}</Box>
          <Box textAlign="center">You will have to provide this code to the giver in order to receive the goods.</Box>
        </ModalBody>
      </ModalContent>
    </Modal>
  );
}

export function ReservationShowPin({ reservation }: { reservation: ICreatorReservation }) {
  const { isOpen, onClose, onOpen } = useDisclosure();

  return (
    <>
      <Button onClick={onOpen}>Show PIN</Button>
      <ReservationPinDialog isOpen={isOpen} onClose={onClose} reservation={reservation} />
    </>
  );
}