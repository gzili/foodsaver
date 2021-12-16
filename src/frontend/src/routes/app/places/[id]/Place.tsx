import { Avatar, Box, Button, Flex, Grid, Heading, HStack, Square, Text, VStack } from '@chakra-ui/react';
import { IconDefinition } from 'components';
import { faCheck, FaIcon, faMapMarkerAlt, faShoppingBag, LoadingOverlay } from 'components';
import api from 'contexts/apiContext';
import { formatDistanceToNowStrict, isPast, parseJSON } from 'date-fns';
import { IOfferDto } from 'dto/offer';
import { UserType } from 'dto/user';
import { absPath, getUserTypeFaIcon, getUserTypeString } from 'helpers';
import { Fragment } from 'react';
import { useInfiniteQuery, useQuery } from 'react-query';
import { useParams, Link as RouterLink } from 'react-router-dom';

interface IHasId {
  id: string;
}

function useIdParam() {
  const { id } = useParams<IHasId>();
  return parseInt(id, 10);
}

interface IStat {
  title: string,
  faIcon: IconDefinition,
  colorScheme: 'brand' | 'green',
  value: number,
}

function PlaceStat(props: IStat) {
  const {
    title,
    faIcon,
    colorScheme,
    value,
  } = props;

  return (
    <Flex direction="column" p={4} borderRadius="lg" boxShadow="rgba(0, 0, 0, 0.1) 0px 1px 3px 0px, rgba(0, 0, 0, 0.06) 0px 0px 2px 0px">
      <Text flex={1} lineHeight={1.15}>{title}</Text>
      <HStack pt={2}>
        <Square bg={`${colorScheme}.50`} borderRadius="md" color={`${colorScheme}.500`} size={8}>
          <Box as={FaIcon} icon={faIcon} />
        </Square>
        <Text fontWeight="bold" fontSize="2xl">{value}</Text>
      </HStack>
    </Flex>
  );
}

interface IPlace {
  id: number,
  activeOffersCount: number,
  completedReservationsCount: number,
  address: {
    city: string,
    street: string,
  },
  avatarPath: string | null,
  name: string,
  type: UserType,
}

const fetchPlace = (id: number) => api.get('users/' + id).json<IPlace>();

function PlaceProfile() {
  const id = useIdParam();

  const { data: place } = useQuery(['place', id], () => fetchPlace(id));

  if (!place) {
    return <LoadingOverlay message='Loading place profile' />;
  }

  return (
    <Box>
      <VStack>
        <VStack spacing={0}>
          <Avatar size="2xl" name={place.name} src={absPath(place.avatarPath)}/>
          <Heading as="h1">{place.name}</Heading>
        </VStack>
        <VStack spacing={0} color="gray.600">
          <HStack spacing={1}>
            <Box as={FaIcon} icon={getUserTypeFaIcon(place.type)} />
            <Text>{getUserTypeString(place.type)}</Text>
          </HStack>
          <HStack spacing={1}>
            <Box as={FaIcon} icon={faMapMarkerAlt} />
            {<Text>{[place.address.street, place.address.city].join(', ')}</Text>}
          </HStack>
        </VStack>
        <Grid pt={2} gap={2} templateColumns="1fr 1fr">
          <PlaceStat title="Active offers" colorScheme="brand" faIcon={faShoppingBag} value={place.activeOffersCount} />
          <PlaceStat title="Completed reservations" colorScheme="green" faIcon={faCheck} value={place.completedReservationsCount} />
        </Grid>
      </VStack>
    </Box>
  );
}

interface IOffersListItem {
  item: IOfferDto,
}

function OffersListItem(props: IOffersListItem) {
  const { item } = props;

  const isExpired = isPast(parseJSON(item.expiresAt));
  const toNow = formatDistanceToNowStrict(parseJSON(item.createdAt), { addSuffix: true });

  return (
    <Box
      as={RouterLink}
      to={'/app/offers/' + item.id}
      pos="relative"
      w="100%"
      h="180px"
      overflow="hidden"
      bg={`url('${absPath(item.food.imagePath, true)}')`}
      bgPos="center"
      bgSize="cover"
      borderRadius="xl"
      opacity={isExpired ? 0.5 : undefined}
      pointerEvents={isExpired ? 'none' : undefined}
    >
      <Flex direction="column" justify="space-between" pos="absolute" inset={0} p={4} bg="rgba(0, 0, 0, 0.4)" color="white">
        <Box>
          <Flex align="center">
            <Avatar size="sm" name={item.giver.username} src={absPath(item.giver.avatarPath)} mr={2} />
            {item.giver.username}
          </Flex>
        </Box>
        <Box>
          <Box fontSize="xs" color="rgba(255, 255, 255, 0.8)">{[item.quantity, item.food.unit, '•', toNow].join(' ')}</Box>
          <Box fontWeight="bold" fontSize="lg">{item.food.name}</Box>
          <HStack spacing={2} fontSize="sm">
            <Box as={FaIcon} icon={faMapMarkerAlt} />
            <Box>{[item.giver.address.street, item.giver.address.city].join(', ')}</Box>
          </HStack>
        </Box>
      </Flex>
    </Box>
  );
}

interface IPaginated<T> {
  data: T[],
  hasNextPage: boolean,
}

const fetchOffers = ({ queryKey, pageParam = 0 }: { queryKey: any[], pageParam?: number }) => {
  const [, { userId }] = queryKey;
  return api.get(`offers?userId=${userId}&limit=3&page=${pageParam}`).json<IPaginated<IOfferDto>>();
}

function PlaceOffers() {
  const id = useIdParam();

  const {
    // isLoading,
    // isError,
    data,
    // error,
    // refetch,
    fetchNextPage,
    hasNextPage,
    isFetchingNextPage
  } = useInfiniteQuery(['offers', { userId: id }], fetchOffers, {
    getNextPageParam: (lastPage, allPages) => lastPage.hasNextPage ? allPages.length : undefined,
  });

  if (!data) {
    return null;
  }

  return (
    <Box mt={2}>
      <Heading fontSize="2xl" textAlign="center">Offers</Heading>
      <VStack mt={1}>
          {data.pages.map((page, i) => (
            <Fragment key={i}>
              {page.data.map(offer => (
                <OffersListItem key={offer.id} item={offer} />
              ))}
            </Fragment>
          ))}
        </VStack>
        {hasNextPage && (
          <Box p={2} textAlign="center">
            <Button isLoading={isFetchingNextPage} onClick={() => fetchNextPage()}>Load more</Button>
          </Box>
        )}
    </Box>
  );
}

export default function Place() {
  return (
    <Box py={2} px={4}>
      <PlaceProfile />
      <PlaceOffers />
    </Box>
  );
}