import type { ReactNode } from 'react';

export const B = ({ children }: { children: ReactNode}) => <span style={{ fontWeight: 'bold' }}>{children}</span>;