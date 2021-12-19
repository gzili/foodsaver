import { Box, Heading, VStack } from '@chakra-ui/react';
import api from 'contexts/apiContext';
import { useAuth } from 'contexts/authContext';
import { useQuery } from 'react-query'
import { IOfferDto } from 'dto/offer';
import { LoadingOverlay, OfferListItem } from 'components';

function OffersList() {
  const { user } = useAuth();
  const { data } = useQuery(['offers', { userId: user?.id }, 'nearby'], () => api.get('my/nearby').json<IOfferDto[]>());

  if (!data) {
    return <LoadingOverlay message="Loading nearby offers" />
  }

  return (
    <VStack>
      {data.map(o => (
        <OfferListItem key={o.id} item={o} />
      ))}
    </VStack>
  );
}

export default function Home() {
  return (
    <Box py={2} px={4}>
      <Heading mb={2}>Nearby offers</Heading>
      <OffersList />
    </Box>
  );
}