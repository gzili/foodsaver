import { Avatar, Box, Flex, Heading, HStack } from '@chakra-ui/react';
import { faHandHoldingHeart, FaIcon, faStore, faUser } from 'components';

import { useAuth } from 'contexts/authContext';
import { UserType } from 'dto/user';
import { absPath } from 'helpers';

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

export default function My() {
  const { user } = useAuth();

  if (!user) {
    return null;
  }

  return (
    <Box py={4} px={4}>
      <Flex direction="column" align="center">
        <Avatar size="2xl" name={user.username} src={absPath(user.avatarPath)}/>
        <Heading as="h1">{user.username}</Heading>
        <UserTypeComponent userType={user.userType} />
      </Flex>
    </Box>
  );
}