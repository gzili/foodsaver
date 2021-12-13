import { Box, BoxProps } from '@chakra-ui/layout';
import type { ReactNode } from 'react';

interface IChildren {
  children: ReactNode,
}

export const B = ({ children }: IChildren) => <span style={{ fontWeight: 'bold' }}>{children}</span>;

export const C = (props: Pick<BoxProps, 'color' | 'children'>) => <Box as="span" {...props} />