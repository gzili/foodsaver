import { ReactNode } from 'react';
import { Box, Heading } from '@chakra-ui/react';

interface IFlowContainer {
  children: ReactNode
}

export const FlowContainer = ({ children }: IFlowContainer) => (
  <Box px={8} pt={6} pb={16} borderTop="1px solid" borderColor="gray.100">
    {children}
  </Box>
);

interface IFlowHeader {
  title: string,
  description: string
}

export function FlowHeader(props: IFlowHeader) {
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

interface IFlowContent {
  children: ReactNode
}

export const FlowContent = ({ children }: IFlowContent) => (
  <Box>{children}</Box>
);