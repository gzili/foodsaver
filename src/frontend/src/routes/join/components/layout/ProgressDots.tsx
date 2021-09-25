import { Box, HStack } from '@chakra-ui/react';

import { faCircle, FaIcon } from 'components/core';

interface IProgressDots {
  count: number,
  activeIndex: number
}

export function ProgressDots(props: IProgressDots) {
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