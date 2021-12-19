import { Avatar, Box, Flex, Grid, Heading, HStack, IconButton, Square, Stack, VStack } from '@chakra-ui/react';
import { faAngleRight, faCheck, faHandHoldingHeart, FaIcon, faMapMarkerAlt, faShoppingBag, faSignOutAlt, faStore, faUser } from 'components';

import { useAuth } from 'contexts/authContext';
import { UserType } from 'dto/user';
import { absPath } from 'helpers';
import { ReactNode } from 'react';
import { useHistory } from 'react-router-dom';

interface IUserTypeComponent {
  userType: UserType
}

function UserTypeComponent({ userType } : IUserTypeComponent) {
  let typeIcon;
  let typeName;

  switch (userType) {
    case UserType.Business:
      typeIcon = <FaIcon icon={faStore} />;
      typeName = 'Business';
      break;
    case UserType.Charity:
      typeIcon = <FaIcon icon={faHandHoldingHeart} />;
      typeName = 'Charity';
      break;
    case UserType.Individual:
      typeIcon = <FaIcon icon={faUser} />
      typeName = 'Individual';
      break;
  }

  return (
    <HStack spacing={1} color="gray.500" fontSize="sm">
      <Box>{typeIcon}</Box>
      <Box>{typeName}</Box>
    </HStack>
  );
}

interface IAccountOption {
  icon: JSX.Element,
  title: string,
  colorScheme?: string,
  details?: string,
  onClick?: () => void,
}

function AccountOption(props: IAccountOption) {
  const {
    icon,
    title,
    colorScheme = 'brand',
    details,
    onClick,
  } = props;

  return (
    <Grid alignItems="center" columnGap={4} templateColumns="auto 1fr auto" w="100%">
      <Square size={10} bg={`${colorScheme}.50`} borderRadius="md" color={`${colorScheme}.500`}>{icon}</Square>
      <Box lineHeight={1.2}>
        <Box fontSize="lg">{title}</Box>
        {details && <Box color="gray.500" fontSize="sm">{details}</Box>}
      </Box>
      <IconButton aria-label="Show my offers" icon={<FaIcon icon={faAngleRight} />} onClick={onClick} />
    </Grid>
  );
}

function LocationOption() {
  const { user } = useAuth();

  return (
    <AccountOption
      colorScheme="blue"
      details={[user?.address.street, user?.address.city].join(', ')}
      icon={<FaIcon icon={faMapMarkerAlt} />}
      title="Location"
    />
  );
}

interface IAccountOptionsGroup {
  children: ReactNode,
  title: string,
}

function AccountOptionsGroup(props: IAccountOptionsGroup) {
  const {
    children,
    title,
  } = props;

  return (
    <Stack mt={2} px={4} spacing={4}>
      <Box>
        <Heading fontSize="xl" mb={2}>{title}</Heading>
        <VStack spacing={4}>{children}</VStack>
      </Box>
    </Stack>
  );
}

export default function My() {
  const { user } = useAuth();
  const history = useHistory();

  if (!user) {
    return null;
  }

  return (
    <Box py={4} px={4}>
      <Flex direction="column" align="center" mb={4}>
        <Avatar size="2xl" name={user.username} src={absPath(user.avatarPath)} />
        <Heading as="h1">{user.username}</Heading>
        <UserTypeComponent userType={user.userType} />
      </Flex>
      <AccountOptionsGroup title="Activity">
        <AccountOption icon={<FaIcon icon={faShoppingBag} />} title="My offers" onClick={() => history.push('/app/my/offers')} />
        <AccountOption colorScheme="green" icon={<FaIcon icon={faCheck} />} title="My reservations" onClick={() => history.push('/app/my/reservations')} />
      </AccountOptionsGroup>
      <AccountOptionsGroup title="Account">
        <LocationOption />
        <AccountOption colorScheme="orange" icon={<FaIcon icon={faUser} />} title="Login details" />
        <AccountOption colorScheme="red" icon={<FaIcon icon={faSignOutAlt} />} title="Sign out" />
      </AccountOptionsGroup>
    </Box>
  );
}