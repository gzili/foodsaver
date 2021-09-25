import { useMemo } from 'react';
import { Control, useController, ControllerRenderProps } from 'react-hook-form';

import Field, { FieldProps } from '../Field';

interface FieldState {
  id: string,
  isInvalid: boolean,
}

interface FieldWithControllerProps extends Omit<FieldProps, 'children'> {
  control: Control,
  name: string,
  children: (props: ControllerRenderProps, state: FieldState) => JSX.Element,
};

const FieldWithController = (props: FieldWithControllerProps) => {
  const {
    children,
    control,
    name,
    ...FieldProps
  } = props;

  const {
    field,
    fieldState: { invalid, error }
  } = useController({ control, name });

  const state = useMemo(() => ({
    id: name,
    isInvalid: invalid,
  }), [name, invalid]);

  return (
    <Field
      id={name}
      isInvalid={invalid}
      errorMessage={invalid ? error?.message : undefined}
      {...FieldProps}
    >
      {children(field, state)}
    </Field>
  );
};

export default FieldWithController;