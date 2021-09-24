import { Box, Button, Flex, Heading, HStack } from '@chakra-ui/react';

import { faCircle, FaIcon } from 'components/core';

import AccountTypeForm from './components/AccountTypeForm';

interface IProgressDots {
  count: number,
  activeIndex: number
}

function ProgressDots(props: IProgressDots) {
  const {
    count,
    activeIndex
  } = props;

  const dotComponents = [];

  for (let i = 0; i < count; ++i) {
    dotComponents[i] = (
      <Box as={FaIcon} key={i} icon={faCircle} fontSize="xs" color={i <= activeIndex ? 'brand.500' : 'brand.100'} />
    );
  }

  return (
    <HStack spacing={4}>
      <HStack spacing={2}>
        {dotComponents}
      </HStack>
      <Box>Step {activeIndex + 1} of {count}</Box>
    </HStack>
  );
}

function Join() {
  return (
    <Box px={8} pt={6} pb={12} borderTop="1px solid" borderColor="gray.100">
      <Box mb={4}>
        <Heading as="h1">Account Type</Heading>
        <Box mt={1} fontSize="sm" lineHeight="1.25" color="gray.500">Choose your account type for optimal experience</Box>
      </Box>
      <Box>
        <AccountTypeForm />
      </Box>
      <Flex
        pos="fixed"
        bottom={0}
        left={0}
        w="100%"
        h={14}
        align="center"
        justify="space-between"
        px={4}
      >
        <ProgressDots count={3} activeIndex={0} />
        <Box>
          <Button h={8} colorScheme="brand">Next</Button>
        </Box>
      </Flex>
    </Box>
  );
}

export default Join;