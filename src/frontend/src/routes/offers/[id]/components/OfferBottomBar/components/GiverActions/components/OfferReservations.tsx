import {
  AlertDialog,
  AlertDialogOverlay,
  AlertDialogContent,
  AlertDialogHeader,
  AlertDialogBody,
  AlertDialogFooter,
  Avatar,
  Box,
  Button,
  Drawer,
  DrawerOverlay,
  DrawerContent,
  DrawerHeader,
  DrawerBody,
  Flex,
  Grid,
  HStack,
  IconButton,
  Stack,
  useDisclosure,
} from '@chakra-ui/react';
import { faCheck, FaIcon } from 'components';
import api from 'contexts/api.context';
import { useHub } from 'contexts/hubContext';
import { formatDistanceToNowStrict } from 'date-fns';
import parseJSON from 'date-fns/parseJSON';
import { useCallback, useEffect, useRef } from 'react';
import { useMutation, useQuery } from 'react-query';
import { useOffer } from 'routes/offers/[id]/contexts/OfferContext';
import { ReservationDto } from '../../../types';

interface ReservationCompletePromptProps {
  isOpen: boolean,
  onClose: () => void,
  reservation: ReservationDto,
}

function ReservationCompletePrompt(props: ReservationCompletePromptProps) {
  const { isOpen, onClose, reservation } = props;

  const cancelButtonRef = useRef<HTMLButtonElement>(null);

  const { mutate } = useMutation({
    mutationFn: () => api.post(`reservations/${reservation.id}/completion`),
    onSuccess: () => onClose(),
  });

  return (
    <AlertDialog isOpen={isOpen} onClose={onClose} leastDestructiveRef={cancelButtonRef} size="xs" isCentered>
      <AlertDialogOverlay />
      <AlertDialogContent>
        <AlertDialogHeader>Confirm pickup</AlertDialogHeader>
        <AlertDialogBody>Do you want to confirm the pickup of this reservation?</AlertDialogBody>
        <AlertDialogFooter>
          <HStack>
            <Button ref={cancelButtonRef} onClick={onClose}>Cancel</Button>
            <Button colorScheme="brand" onClick={() => mutate()}>Confirm</Button>
          </HStack>
        </AlertDialogFooter>
      </AlertDialogContent>
    </AlertDialog>
  );
}

function ReservationComplete({ reservation }: { reservation: ReservationDto }) {
  const { isOpen, onOpen, onClose } = useDisclosure();

  return (
    <>
      <ReservationCompletePrompt isOpen={isOpen} onClose={onClose} reservation={reservation} />
      <IconButton colorScheme="brand" aria-label="Mark as completed" icon={<FaIcon icon={faCheck} />} onClick={onOpen} />
    </>
  );
}

function ReservationsListItem({ reservation }: { reservation: ReservationDto }) {
  const offer = useOffer();

  let distanceString;
  let distanceDate;
  
  if (reservation.completedAt) {
    distanceString = 'Completed ';
    distanceDate = reservation.completedAt;
  } else {
    distanceString = 'Reserved ';
    distanceDate = reservation.createdAt;
  }

  distanceString += formatDistanceToNowStrict(parseJSON(distanceDate), { addSuffix: true });

  return (
    <Grid mt={2} templateColumns="auto 1fr auto" gap={4} alignItems="center" borderRadius="md">
      <Avatar
        name={reservation.user.username}
        src={reservation.user.avatarPath ? '/' + reservation.user.avatarPath : undefined}
      />
      <Box>
        <Box>{reservation.user.username}</Box>
        <Flex align="flex-end">
          <Box fontSize="2xl" fontWeight="bold" color="brand.500" lineHeight={1.15}>{reservation.quantity}</Box>
          <Box ml={1} color="brand.400">{offer.food.unit}</Box>
        </Flex>
        <Box fontSize="sm" color="gray.500">{distanceString}</Box>
      </Box>
      {!reservation.completedAt && (
        <Box>
          <ReservationComplete reservation={reservation} />
        </Box>
      )}
    </Grid>
  );
}

interface ReservationsListProps {
  header: string,
  reservations: ReservationDto[],
}

function ReservationsList(props: ReservationsListProps) {
  const { header, reservations } = props;

  return (
    <Box>
      <Box>{header} ({reservations.length})</Box>
      <Box>
        {reservations.map(r => <ReservationsListItem key={r.id} reservation={r} />)}
      </Box>
    </Box>
  );
}

function Reservations() {
  const offer = useOffer();

  const { data: reservations, refetch } = useQuery({
    queryFn: () => api.get(`offers/${offer.id}/reservations`).json<ReservationDto[]>(),
  });

  const { connection } = useHub();

  const handleReservationsChange = useCallback((offerId: number) => {
    if (offerId === offer.id) {
      refetch();
    }
  }, [refetch, offer.id]);

  useEffect(() => {
    connection.on("ReservationsChanged", handleReservationsChange);

    return () => connection.off("ReservationsChanged", handleReservationsChange);
  }, [handleReservationsChange, connection]);

  if (!reservations) {
    return null;
  }

  const active = [];
  const completed = [];

  for (const r of reservations) {
    r.completedAt ? completed.push(r) : active.push(r);
  }

  return (
    <Stack spacing={4}>
      <ReservationsList header="Active" reservations={active} />
      <ReservationsList header="Completed" reservations={completed} />
    </Stack>
  );
}

interface ReservationsDrawerProps {
  isOpen: boolean,
  onClose: () => void,
}

function ReservationsDrawer(props: ReservationsDrawerProps) {
  const { isOpen, onClose } = props;

  return (
    <Drawer isOpen={isOpen} onClose={onClose} placement="bottom" size="md">
      <DrawerOverlay />
      <DrawerContent>
        <DrawerHeader>Reservations</DrawerHeader>
        <DrawerBody maxH="60vh" pt={0}>
          <Reservations />
        </DrawerBody>
      </DrawerContent>
    </Drawer>
  );
}

export function OfferReservations() {
  const { isOpen, onOpen, onClose } = useDisclosure();

  return (
    <>
      <ReservationsDrawer isOpen={isOpen} onClose={onClose} />
      <Button colorScheme="brand" onClick={onOpen}>Show reservations</Button>
    </>
  );
}