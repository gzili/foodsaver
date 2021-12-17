import { useEffect } from 'react';
import { useQuery } from 'react-query';

import api from 'contexts/apiContext';
import { useHub } from 'contexts/hubContext';

import { useOffer } from '../../../../contexts/OfferContext';
import { ReservationCreate, ReservedActions } from './components';
import type { ICreatorReservation } from './types';

export function UserActions() {
  const { id } = useOffer();

  const { connection } = useHub();

  const { isLoading, data, refetch } = useQuery(['reservation', id], () => {
    return api.get(`offers/${id}/reservation`).json<ICreatorReservation | ''>();
  });

  useEffect(() => {
    const cb = () => refetch();

    connection.on("AvailableQuantityChanged", cb);

    return () => connection.off("AvailableQuantityChanged", cb);
  }, [refetch, connection]);

  if (isLoading) {
    return null;
  }

  return !!data ? <ReservedActions reservation={data} /> : <ReservationCreate />;
}