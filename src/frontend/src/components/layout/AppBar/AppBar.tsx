import { HamburgerIcon } from '@chakra-ui/icons';
import { Avatar, Box, Button, Flex, IconButton, Menu, MenuButton, MenuDivider, MenuItem, MenuList } from '@chakra-ui/react';
import { Link } from 'react-router-dom';

import { faHandHoldingHeart, FaIcon, faSignOutAlt } from 'components';
import { useAuth } from 'contexts/authContext';
import { absPath } from 'helpers';

export default function AppBar() {
  const { user, signOut } = useAuth();

  return (
    <Flex
      pos="fixed"
      top={0}
      right={0}
      left={0}
      h={10}
      zIndex="docked"
      justify="space-between"
      align="center"
      px={2}
      bg="white"
      borderBottom="1px solid"
      borderBottomColor="gray.100"
    >
      <IconButton h={8} minW={8} variant="ghost" icon={<HamburgerIcon />} aria-label="Open menu" />
      <Link to="/">
        <Box fontSize="xl" fontWeight="bold">
          food
          <Box as="span" color="brand.500">saver</Box>
        </Box>
      </Link>
      {user ? (
        <Menu autoSelect={false} gutter={16} placement="bottom-end">
          <MenuButton>
            <Avatar name={user.username} src={absPath(user.avatarPath)} size="xs" />
          </MenuButton>
          <MenuList>
            <MenuItem icon={<FaIcon icon={faHandHoldingHeart} />}>My offers</MenuItem>
            <MenuDivider />
            <MenuItem icon={<FaIcon icon={faSignOutAlt} />} onClick={signOut}>Sign out</MenuItem>
          </MenuList>
        </Menu>
      ) : (
        <Button as={Link} to="/login" size="xs">Sign in</Button>
      )}
    </Flex>
  );
}