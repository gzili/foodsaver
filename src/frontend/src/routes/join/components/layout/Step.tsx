import { ReactNode } from 'react';
import { Box, Heading } from '@chakra-ui/react';

interface IStepContainer {
  children: ReactNode
}

export const StepContainer = ({ children }: IStepContainer) => (
  <Box px={8} pt={6} pb={16} borderTop="1px solid" borderColor="gray.100">
    {children}
  </Box>
);

interface IStepHeader {
  title: string,
  description: string
}

export function StepHeader(props: IStepHeader) {
  const {
    title,
    description
  } = props;

  return (
    <Box mb={4}>
      <Heading as="h1">{title}</Heading>
      <Box mt={1} fontSize="sm" lineHeight="1.25" color="gray.500">{description}</Box>
    </Box>
  );
}

interface IStepContent {
  children: ReactNode
}

export const StepContent = ({ children }: IStepContent) => (
  <Box>{children}</Box>
);