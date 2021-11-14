import { useCallback, useEffect } from 'react';
import { useParams } from 'react-router-dom';
import { useQuery, useQueryClient } from 'react-query';
import { formatDistanceToNowStrict, parseJSON } from 'date-fns';
import { Box, Code, Heading, Image, Text, Flex, Avatar, VStack } from '@chakra-ui/react';

import { IOfferDto } from 'dto/offer';
import { LoadingOverlay } from 'components/layout';
import { useAuth } from 'contexts/auth.context';
import { useHub } from 'contexts/hubContext';
import { OfferProvider } from './contexts/OfferContext';
import { ReservationBar } from './components/ReservationBar/ReservationBar';

async function fetchOfferById(id: string) {
  if (!/\d+/.test(id)) {
    throw new Error(`Invalid offer id \`${id}\``)
  }

  const res = await fetch(`/api/offers/${id}`);

  if (!res.ok) {
    throw new Error(`Server responded with status ${res.status} ${res.statusText}`);
  }

  return await res.json();
}

export default function Offer() {
  const { id } = useParams<{ id: string }>();

  const { user } = useAuth();

  const { connection, isConnected } = useHub();

  const { isLoading, isError, data: offer, error } = useQuery<IOfferDto>(['offer', id], () => fetchOfferById(id), {
    refetchOnWindowFocus: false,
  });

  const queryClient = useQueryClient();

  const handleAvailableQuantityChange = useCallback((availableQuantity: number) => {
    queryClient.setQueryData(['offer', id], (offer: any) => {
      return {...offer, availableQuantity};
    });
  }, [id, queryClient]);

  useEffect(() => {
    if (isConnected) {
      connection.invoke("ListenOffer", id).then(() => {
        console.log("Listening on offer %s", id);
      }).catch(err => console.log(err));
    }

    connection.on('AvailableQuantityChanged', handleAvailableQuantityChange);

    return () => {
      connection.off('AvailableQuantityChanged', handleAvailableQuantityChange);
    }
  }, [isConnected, id, handleAvailableQuantityChange, connection]);

  if (isLoading) {
    return <LoadingOverlay message="Loading offer" />;
  }

  if (isError) {
    return <>{(error as Error).message}</>;
  }

  if (!offer) {
    return null;
  }

  const mapsApiKey = undefined;//process.env.REACT_APP_GOOGLE_MAPS_API_KEY;
  const encodedAddress = encodeURIComponent([offer.giver.address.street, offer.giver.address.city].join(', '));
  const createdToNow = formatDistanceToNowStrict(parseJSON(offer.createdAt), { addSuffix: true });
  const [expNum, expUnit] = formatDistanceToNowStrict(parseJSON(offer.expiresAt)).split(' ');

  return (
    <OfferProvider value={offer}>
      <Box>
        <Image w="100%" h="200px" objectFit="cover" src={'/' + offer.food.imagePath} alt="Offer image" />
        <VStack py={4} px={6} spacing={4} direction="row" align="normal">
          <Box>
            <Heading as="h1" fontSize="2xl" lineHeight="1">{offer.food.name}</Heading>
            <Text fontSize="sm" color="gray.500">{createdToNow}</Text>
          </Box>
          <Box px={4} py={6} background="brand.50" borderRadius="lg">
            <Flex align="flex-end" mb={4}>
              <Box fontSize="4xl" fontWeight="bold" color="brand.500" lineHeight={0.8}>{offer.availableQuantity}</Box>
              <Box paddingLeft="2px" fontWeight="bold" lineHeight={1} color="brand.400">/{offer.quantity} {offer.food.unit} available</Box>
            </Flex>
            <Flex align="flex-end">
              <Box fontSize="4xl" fontWeight="bold" color="brand.500" lineHeight={0.8}>{expNum}</Box>
              <Box paddingLeft="2px" fontWeight="bold" lineHeight={1} color="brand.400">{expUnit} left</Box>
            </Flex>
          </Box>
          <Flex align="center">
            <Avatar size="sm" name={offer.giver.username} />
            <Box ml={2}>{offer.giver.username}</Box>
          </Flex>
          <Box>
            <Box fontWeight="bold">Description</Box>
            <Box>{offer.description}</Box>
          </Box>
          <Box>
            <Heading mb={1} fontSize="md" fontWeight="bold">Location</Heading>
            {mapsApiKey ? (
              <Box as="iframe"
                w="100%"
                h="250px"
                borderRadius="md"
                title="Location map"
                src={`https://www.google.com/maps/embed/v1/place?key=${mapsApiKey}&q=${encodedAddress}`}
              />
            ) : (
              <Text fontSize="sm">
                Google Maps API key is kept private to prevent abuse by third parties.
                Please create <Code fontSize="xs">.env.local</Code> file in <Code fontSize="xs">frontend</Code> directory
                and insert line <Code fontSize="xs" mb="2px">REACT_APP_GOOGLE_MAPS_API_KEY={'<YOUR_API_KEY>'}</Code>
                replacing <Code fontSize="xs">YOUR_API_KEY</Code> with valid Google Maps API key in order
                for map to load. Double check the filename, so the file is not uploaded by git.
              </Text>
            )}
            </Box>
        </VStack>
        {user && <Box h={14} />}
      </Box>
      {user && <ReservationBar />}
    </OfferProvider>
  );
}