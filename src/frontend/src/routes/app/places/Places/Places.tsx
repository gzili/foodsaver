import { Avatar, Box, Flex, Grid, Heading, HStack, IconButton, Text, VStack } from '@chakra-ui/react';
import { useQuery } from 'react-query';
import { Link } from 'react-router-dom';

import { B, FaIcon, faLongArrowAltRight, faMapMarkerAlt, faShoppingBag, LoadingOverlay } from 'components';
import api from 'contexts/apiContext';
import { UserType } from 'dto/user';
import { absPath, getUserTypeFaIcon, getUserTypeString } from 'helpers';

interface IPlace {
  id: number,
  username: string,
  userType: UserType,
  avatarPath: string | null,
  address: {
    street: string,
    city: string,
  },
  activeOffersCount: number,
}

interface IPlacesListItem {
  place: IPlace,
}

const shouldPluralize = (count: number) => {
  return (count > 1 && count < 21) || (count % 10 !== 1);
}

function PlacesListItem(props: IPlacesListItem) {
  const { place } = props;

  return (
    <Box p={4} bg="white" borderRadius="lg" boxShadow="rgba(0, 0, 0, 0.1) 0px 1px 3px 0px, rgba(0, 0, 0, 0.06) 0px 0px 2px 0px">
      <Grid templateColumns="1fr max-content" columnGap={2}>
        <HStack spacing={2}>
          <Avatar src={absPath(place.avatarPath)} name={place.username} />
          <Box>
            <Text fontWeight="bold">{place.username}</Text>
            <Flex align="center" lineHeight={1.1} color="gray.500">
              <HStack spacing={1} align="flex-start">
                <Box as={FaIcon} icon={getUserTypeFaIcon(place.userType)} fontSize="xs" />
                <Text fontSize="sm">{getUserTypeString(place.userType)}</Text>
              </HStack>
            </Flex>
          </Box>
        </HStack>
        <IconButton as={Link} aria-label="Visit place" icon={<FaIcon icon={faLongArrowAltRight} />} to={"/app/places/" + place.id} />
      </Grid>
      <Box mt={2} color="gray.600">
        <HStack spacing={1}>
          <Box as={FaIcon} color="brand.500" icon={faShoppingBag} fixedWidth />
          <Text><B>{place.activeOffersCount}</B> active offer{shouldPluralize(place.activeOffersCount) ? 's' : undefined}</Text>
        </HStack>
        <HStack spacing={1}>
          <Box as={FaIcon} color="brand.500" icon={faMapMarkerAlt} fixedWidth />
          <Text>{[place.address.street, place.address.city].join(', ')}</Text>
        </HStack>
      </Box>
    </Box>
  );
}

const fetchPlaces = () => api.get('summary/givers').json<IPlace[]>();

function PlacesList() {
  const { isLoading, data: places } = useQuery('places', fetchPlaces);

  if (isLoading) {
    return <LoadingOverlay message="Loading places" />
  }

  if (!places) {
    return null;
  }

  const listItems = places.map(place => (
    <PlacesListItem key={place.id} place={place} />
  ));

  return (
    <VStack spacing={2} sx={{ alignItems: false }}>
      {listItems}
    </VStack>
  );
}

export default function Places() {
  return (
    <Box py={2} px={4}>
      <Heading as="h1">Places</Heading>
      <PlacesList />
    </Box>
  );
}