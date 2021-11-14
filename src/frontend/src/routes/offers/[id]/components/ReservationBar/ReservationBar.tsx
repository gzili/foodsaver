import { Flex } from '@chakra-ui/react';
import api from 'contexts/api.context';
import { useHub } from 'contexts/hubContext';
import { useEffect } from 'react';
import { useQuery } from 'react-query';
import { useOffer } from '../../contexts/OfferContext';
import { ReservationCreate } from './components/ReservationCreate';
import { ReservationDelete } from './components/ReservationDelete';
import type { ReservationDto } from './types';

export function ReservationBar() {
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

  return (
    <Flex
      pos="fixed"
      bottom={0}
      left={0}
      right={0}
      h={14}
      px={4}
      direction="column"
      justify="center"
      bg="white"
      boxShadow="0 0 5px 0 rgba(0, 0, 0, 0.1)"
    >
      {!!data ? (
        <ReservationDelete reservation={data} />
      ) : (
        <ReservationCreate />
      )}
    </Flex>
  );
}