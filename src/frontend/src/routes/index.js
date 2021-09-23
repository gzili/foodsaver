import { Box, Button, Flex, Grid, GridItem, Heading, HStack, Image, Link, Text } from '@chakra-ui/react';
import { Link as RouterLink } from 'react-router-dom';

import logo from 'resources/foodsaver_logo.jpg';

export default function Home() {
  return (
    <>
      <Box
        pos="fixed"
        zIndex="docked"
        w="100%"
        bg="white"
        borderBottom="1px solid"
        borderBottomColor="gray.100"
      >
        <Flex justify="space-between" align="center" maxW="container.xl" m="0 auto" px={10} py={2}>
          <Box as={RouterLink} to="/" fontSize="2xl" fontWeight="bold">
            Food
            <Box as="span" color="brand.500">Saver</Box>
          </Box>
          <HStack spacing={8}>
            <HStack spacing={4} align="center">
              <Link as={RouterLink} to="/offers">Offers</Link>
              <Link as={RouterLink} to="/places">Places</Link>
            </HStack>
            <HStack spacing={2}>
              <Button as={RouterLink} to="/login" size="sm">Login</Button>
              <Button as={RouterLink} to="/join" colorScheme="brand" size="sm">Join</Button>
            </HStack>
          </HStack>
        </Flex>
      </Box>
      <Grid m="0 auto" p={10} h="100vh" maxW="container.xl" alignItems="center" templateColumns="60% 1fr" columnGap={10}>
        <GridItem>
          <Heading as="h1" fontSize="5rem" fontWeight="black" lineHeight="1.0">
            Where excess food gets shared
          </Heading>
          <Text fontSize="xl" py={4}>
            Make food waste a thing of the past by joining a platform where everyone from individuals to large food
            chains can share excess food with anyone.
          </Text>
          <HStack spacing={4}>
            <Button as={RouterLink} to="/join" size="lg" colorScheme="brand">Join</Button>
            <Button as={RouterLink} to="/offers" size="lg">Browse offers</Button>
          </HStack>
        </GridItem>
        <GridItem>
          <Image src={logo} w="100%" />
        </GridItem>
      </Grid>
    </>
  );
}