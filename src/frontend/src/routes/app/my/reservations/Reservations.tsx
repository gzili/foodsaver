import { Box, Heading, VStack } from '@chakra-ui/react';
import api from 'contexts/apiContext';
import { useAuth } from 'contexts/authContext';
import { useQuery } from 'react-query'
import { IOfferDto } from 'dto/offer';
import { LoadingOverlay, OfferListItem } from 'components';

function ReservationsList() {
  const { user } = useAuth();
  const { data } = useQuery(['reservations', { userId: user?.id }], () => api.get('my/reservations').json<IOfferDto[]>());

  if (!data) {
    return <LoadingOverlay message="Loading reservations" />
  }

  return (
    <VStack>
      {data.map(o => (
        <OfferListItem key={o.id} item={o} />
      ))}
    </VStack>
  );
}

export default function Reservations() {
  return (
    <Box py={2} px={4}>
      <Heading mb={2}>My reservations</Heading>
      <ReservationsList />
    </Box>
  );
}