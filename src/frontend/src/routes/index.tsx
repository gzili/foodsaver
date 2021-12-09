import { Box, Button, Flex, Heading, HStack, Image, Text } from '@chakra-ui/react';
import { C } from 'components';
import { Link as RouterLink } from 'react-router-dom';

import logo from 'resources/foodsaver_logo.png';

export default function Home() {
  return (
    <Box h="100vh" pt={10}>
      <Flex p={4} h="100%" direction="column" justifyContent="center">
        <Box>
          <Box flexShrink={1}>
            <Image src={logo} maxW="250px" m="0 auto" />
          </Box>
          <Heading as="h1" maxW="300px" m="0 auto" pt={2} textAlign="center">
            <C color="brand.500">Share</C> excess <C color="brand.500">food</C> with <C color="brand.500">anyone</C>
          </Heading>
          <Text textAlign="center" pt={2}>
            Join the platform where everyone from individuals to large food chains
            can share excessive food with anyone. Make food waste a thing of the past.
          </Text>
          <HStack justifyContent="center" spacing={2} pt={4}>
            <Button as={RouterLink} to="/join" colorScheme="brand">Join</Button>
            <Button as={RouterLink} to="/app/offers">Browse offers</Button>
          </HStack>
        </Box>
      </Flex>
    </Box>
  );
}