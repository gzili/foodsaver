import { Grid } from '@chakra-ui/react';
import { ReservationDelete } from '.';
import { ICreatorReservation } from '../types';
import { ReservationShowPin } from './ReservationShowPin';

export function ReservedActions({ reservation }: { reservation: ICreatorReservation }) {
  return (
    <Grid gap={2} templateColumns="1fr 1fr">
      <ReservationShowPin reservation={reservation} />
      <ReservationDelete reservation={reservation} />
    </Grid>
  );
}