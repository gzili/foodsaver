import { AddIcon } from '@chakra-ui/icons';
import { Box, Flex, Grid, IconButton, useDisclosure } from '@chakra-ui/react';
import { faHamburger, faHome, FaIcon, faMapMarkerAlt, faUser } from 'components';
import { ReactNode } from 'react';
import { useRouteMatch } from 'react-router';
import { Link } from 'react-router-dom';
import { CreateOfferDrawer } from 'routes/app/offers/Offers/components/CreateOfferDrawer';

interface INavButton {
  icon: ReactNode,
  text: string,
  to: string,
}

function NavButton(props: INavButton) {
  const {
    icon,
    text,
    to,
  } = props;

  const match = useRouteMatch(to);

  return (
    <Link to={to}>
      <Flex
        h="100%"
        direction="column"
        justify="center"
        align="center"
        color="gray.600"
        _activeLink={{ color: 'brand.500' }}
        aria-current={match != null ? "page" : undefined}
      >
        <Box
          fontSize="lg"
          lineHeight={1}
        >
          {icon}
        </Box>
        <Box
          fontSize="sm"
          lineHeight={1}
          mt={1}
        >
          {text}
        </Box>
      </Flex>
    </Link>
  );
}

function CreateButton() {
  const { isOpen, onOpen, onClose } = useDisclosure();

  return (
    <>
      <Flex h="100%" justify="center" align="center">
        <IconButton
          aria-label="Create an offer"
          colorScheme="brand"
          isRound
          icon={<AddIcon />}
          onClick={onOpen}
          size="md"
        />
      </Flex>
      <CreateOfferDrawer isOpen={isOpen} onClose={onClose} />
    </>
  );
}

const BAR_HEIGHT = 16;

export default function BottomBar() {
  return (
    <>
      <Box h={BAR_HEIGHT} />
      <Flex
        sx={{
          pos: 'fixed',
          bottom: 0,
          left: 0,
          right: 0,
          h: BAR_HEIGHT,
          px: 4,
          bg: 'white',
          alignItems: 'center',
          justifyContent: 'center',
          boxShadow: '0 0 5px 0 rgba(0, 0, 0, 0.1)'
        }}
      >
        <Grid w="100%" h="100%" templateColumns="1fr 1fr 1fr 1fr 1fr">
          {<NavButton to="/app/home" icon={<FaIcon icon={faHome} />} text="Home" />}
          <NavButton to="/app/offers" icon={<FaIcon icon={faHamburger} />} text="Offers" />
          <CreateButton />
          <NavButton to="/app/places" icon={<FaIcon icon={faMapMarkerAlt} />} text="Places" />
          {<NavButton to="/app/my" icon={<FaIcon icon={faUser} />} text="Account" />}
        </Grid>
      </Flex>
    </>
  );
}