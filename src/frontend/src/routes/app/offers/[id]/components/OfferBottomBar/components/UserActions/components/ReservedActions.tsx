import { Grid } from '@chakra-ui/react';
import { ReservationCompleted, ReservationDelete, ReservationShowPin } from '.';
import { ICreatorReservation } from '../types';

export function ReservedActions({ reservation }: { reservation: ICreatorReservation }) {
  if (reservation.completedAt === null) {
    return (
      <Grid gap={2} templateColumns="1fr 1fr">
        <ReservationShowPin reservation={reservation} />
        <ReservationDelete reservation={reservation} />
      </Grid>
    );
  } else {
    return <ReservationCompleted reservation={reservation} />
  }
}