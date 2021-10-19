import { forwardRef } from 'react';
import { Rifm } from 'rifm';

import Input, { InputProps } from '../Input';

interface RifmArgs {
  format: (str: string) => string;
  replace?: (str: string) => string;
  append?: (str: string) => string;
  mask?: boolean;
  accept?: RegExp;
}

export interface RifmInputProps extends Omit<InputProps, 'onChange'> {
  rifmProps: RifmArgs,
  value: string,
  onChange: (str: string) => void,
}

const RifmInput = forwardRef<HTMLInputElement, RifmInputProps>((props, ref) => {
  const {
    rifmProps,
    value,
    onChange,
    ...InputProps
  } = props;

  return (
    <Rifm {...rifmProps} value={value} onChange={onChange}>
      {({ value, onChange }) => (
        <Input {...InputProps} ref={ref} value={value} onChange={onChange} />
      )}
    </Rifm>
  );
});

export default RifmInput;