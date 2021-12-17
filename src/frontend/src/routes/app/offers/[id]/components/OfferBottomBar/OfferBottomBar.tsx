import { Flex } from '@chakra-ui/react';

import { useAuth } from 'contexts/authContext';

import { useOffer } from '../../contexts/OfferContext';
import { GiverActions, UserActions } from './components';

export function OfferBottomBar() {
  const { user } = useAuth();
  const offer = useOffer();

  return (
    <Flex
      pos="fixed"
      bottom={0}
      left={0}
      right={0}
      h={14}
      px={4}
      direction="column"
      justify="center"
      bg="white"
      boxShadow="0 0 5px 0 rgba(0, 0, 0, 0.1)"
    >
      {(user?.id === offer.giver.id) ? <GiverActions /> : <UserActions />}
    </Flex>
  );
}