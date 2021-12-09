import { Box, Flex, Spinner } from '@chakra-ui/react';

export default function LoadingOverlay({ message }: { message: string }) {
  return (
    <Flex
      pos="fixed"
      zIndex={-1}
      w="100%"
      h="100%"
      justify="center"
      align="center"
    >
      <Flex direction="column" align="center">
        <Spinner />
        <Box mt={2}>{message}</Box>
      </Flex>
    </Flex>
  );
}