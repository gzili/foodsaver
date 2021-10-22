import { Fragment } from 'react';
import { useParams } from 'react-router-dom';
import { useQuery } from 'react-query';
import { format, formatDistanceToNowStrict, parseJSON } from 'date-fns';
import { Box, Code, Grid, GridItem, Heading, Image, Text } from '@chakra-ui/react';

import { IOfferDto } from 'dto/offer';
import { LoadingOverlay } from 'components/layout';

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

interface IOfferGridItem {
  title: string,
  value: string,
}

export default function Offer() {
  const { id } = useParams<{ id: string }>();

  const { isLoading, isError, data: offer, error } = useQuery<IOfferDto>(['todos', id], () => fetchOfferById(id), {
    refetchOnWindowFocus: false,
  });

  if (isLoading) {
    return <LoadingOverlay message="Loading offer" />;
  }

  if (isError) {
    return <>{(error as Error).message}</>;
  }

  if (!offer) {
    return null;
  }

  const offerGridItems: IOfferGridItem[] = [
    {
      title: 'Quantity',
      value: [offer.quantity, offer.food.unit].join(' '),
    },
    {
      title: 'Expiration date',
      value: format(parseJSON(offer.expirationDate), 'yyyy-MM-dd HH:mm'),
    },
  ];

  const mapsApiKey = process.env.REACT_APP_GOOGLE_MAPS_API_KEY;
  const encodedAddress = encodeURIComponent([offer.giver.address.streetAddress, offer.giver.address.city].join(', '));
  const createdToNow = formatDistanceToNowStrict(parseJSON(offer.creationDate), { addSuffix: true });

  return (
    <Box>
      <Image w="100%" h="200px" objectFit="cover" src={'/' + offer.food.imagePath} alt="Offer image" />
      <Box py={4} px={6}>
        <Heading as="h1" fontSize="2xl" lineHeight="1">{offer.food.name}</Heading>
        <Text mb={4} fontSize="sm" color="gray.600">{createdToNow}</Text>
        <Grid mb={4} templateColumns="1fr auto" columnGap={2} rowGap={2}>
          {offerGridItems.map((item, index) => (
            <Fragment key={index}>
              <GridItem rowStart={index + 1} colStart={1} fontWeight="bold">{item.title}</GridItem>
              <GridItem rowStart={index + 1} colStart={2} textAlign="right">{item.value}</GridItem>
            </Fragment>
          ))}
        </Grid>
        <Box mb={4}>
          <Heading mb={1} fontSize="md" fontWeight="bold">Description</Heading>
          <Text>{offer.description}</Text>
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
      </Box>
    </Box>
  );
}