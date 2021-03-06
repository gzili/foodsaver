import { ReactNode } from 'react';
import { Flex } from '@chakra-ui/react';

interface IBottomBar {
  children: ReactNode
}

export function BottomBar(props: IBottomBar) {
  const { children } = props;

  return (
    <Flex
      pos="fixed"
      bottom={0}
      left={0}
      w="100%"
      h={16}
      px={4}
      align="center"
      justify="space-between"
      bg="white"
    >
      {children}
    </Flex>
  );
}