import { Box, HStack } from '@chakra-ui/react';

import { faCircle, FaIcon } from 'components/core';

interface IProgressIndicator {
  count: number,
  activeIndex: number
}

export function ProgressIndicator(props: IProgressIndicator) {
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
    <HStack spacing={2}>
      <HStack spacing={1}>
        {dotComponents}
      </HStack>
      <Box>Step {activeIndex + 1} of {count}</Box>
    </HStack>
  );
}