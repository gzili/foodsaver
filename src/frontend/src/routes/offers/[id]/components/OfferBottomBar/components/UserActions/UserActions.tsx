import api from 'contexts/api.context';
import { useHub } from 'contexts/hubContext';
import { useEffect } from 'react';
import { useQuery } from 'react-query';
import { useOffer } from '../../../../contexts/OfferContext';
import { ReservationCreate, ReservationDelete } from './components';
import type { ReservationDto } from '../.././types';

export function UserActions() {
  const { id } = useOffer();

  const { connection } = useHub();

  const { isLoading, data, refetch } = useQuery(['reservation', id], () => {
    return api.get(`offers/${id}/reservation`).json<ReservationDto | ''>();
  });

  useEffect(() => {
    const cb = () => refetch();

    connection.on("AvailableQuantityChanged", cb);

    return () => connection.off("AvailableQuantityChanged", cb);
  }, [refetch, connection]);

  if (isLoading) {
    return null;
  }

  return !!data ? <ReservationDelete reservation={data} /> : <ReservationCreate />;
}