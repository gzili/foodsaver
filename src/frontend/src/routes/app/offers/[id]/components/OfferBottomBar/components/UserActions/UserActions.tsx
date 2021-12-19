import { useCallback, useEffect } from 'react';
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

  const handleRefetch = useCallback(() => refetch(), [refetch]);

  const handleReservationCompletion = useCallback(id => {
    if (data && data.id === id) {
      refetch();
    }
  }, [data, refetch]);

  useEffect(() => {
    connection.on("AvailableQuantityChanged", handleRefetch);
    connection.on("ReservationCompleted", handleReservationCompletion);

    return () => {
      connection.off("AvailableQuantityChanged", handleRefetch);
      connection.off("ReservationCompleted", handleReservationCompletion);
    };
  }, [refetch, handleRefetch, handleReservationCompletion, connection]);

  if (isLoading) {
    return null;
  }

  return !!data ? <ReservedActions reservation={data} /> : <ReservationCreate />;
}