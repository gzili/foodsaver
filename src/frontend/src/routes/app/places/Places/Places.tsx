import { Avatar, Box, Flex, Heading, VStack } from '@chakra-ui/react';
import { faHamburger, FaIcon, faMapMarkerAlt, faTag, LoadingOverlay } from 'components';
import api from 'contexts/apiContext';
import { absPath } from 'helpers';
import { useQuery } from 'react-query';
import { Link } from 'react-router-dom';

interface IPlace {
  id: number,
  username: string,
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
  if ((count > 1 && count < 21) || (count % 10 !== 1)) {
    return true;
  } else {
    return false;
  }
}

function PlacesListItem(props: IPlacesListItem) {
  const { place } = props;

  return (
    <Link to={'/app/places/' + place.id}>
      <Flex
        align="center"
        p={4}
        borderColor="gray.100"
        borderStyle="solid"
        borderWidth="1px"
        borderRadius="lg"
      >
        <Avatar size="md" name={place.username} src={absPath(place.avatarPath)} />
        <Box ml={4} color="gray.600">
          <Flex align="center">
            <Box as={FaIcon} icon={faTag} fixedWidth mr={2} fontSize="sm" color="brand.500" />
            {place.username}
          </Flex>
          <Flex align="center">
            <Box as={FaIcon} icon={faMapMarkerAlt} fixedWidth mr={2} fontSize="sm" color="brand.500" />
            {[place.address.street, place.address.city].join(', ')}
          </Flex>
          <Flex align="center">
            <Box as={FaIcon} icon={faHamburger} fixedWidth mr={2} fontSize="sm" color="brand.500" />
            {place.activeOffersCount} offer{shouldPluralize(place.activeOffersCount) && 's'}
          </Flex>
        </Box>
      </Flex>
    </Link>
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
    <Box pt={12} pb={2} px={4}>
      <Heading as="h1">Places</Heading>
      <PlacesList />
    </Box>
  );
}