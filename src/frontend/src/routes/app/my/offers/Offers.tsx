import { Box, Heading, VStack } from '@chakra-ui/react';
import api from 'contexts/apiContext';
import { useAuth } from 'contexts/authContext';
import { useQuery } from 'react-query'
import { IOfferDto } from 'dto/offer';
import { LoadingOverlay, OfferListItem } from 'components';

function OffersList() {
  const { user } = useAuth();
  const { data } = useQuery(['offers', { userId: user?.id }], () => api.get('my/offers').json<IOfferDto[]>());

  if (!data) {
    return <LoadingOverlay message="Loading your offers" />
  }

  return (
    <VStack>
      {data.map(o => (
        <OfferListItem key={o.id} item={o} />
      ))}
    </VStack>
  );
}

export default function Offers() {
  return (
    <Box py={2} px={4}>
      <Heading mb={2}>My offers</Heading>
      <OffersList />
    </Box>
  );
}