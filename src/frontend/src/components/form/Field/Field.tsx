import { Children } from 'react';
import {
  Box,
  chakra,
  FormControl,
  FormErrorMessage,
  FormHelperText,
  FormLabel,
  FormControlProps,
} from '@chakra-ui/react';
import FaIcon, { faExclamationCircle, faInfoCircle } from 'components/core/FaIcon';

export interface FieldProps extends Omit<FormControlProps, 'isRequired'> {
  label: string,
  children: JSX.Element,
  helperText?: string,
  errorMessage?: string,
  optional?: boolean,
}

export const RequiredIndicator = (
  <chakra.span ml={1} color="brand.500">
    <sup>•</sup>
  </chakra.span>
);

const FormField = (props: FieldProps) => {
  const {
    label,
    children,
    helperText,
    errorMessage,
    optional,
    ...FormFieldProps
  } = props;

  return (
    <FormControl isRequired={!optional} {...FormFieldProps}>
      <FormLabel mb={0.5} fontWeight="bold" requiredIndicator={RequiredIndicator}>{label}</FormLabel>
      {Children.only(children)}
      {helperText && !FormFieldProps.isInvalid && (
        <FormHelperText display="flex" mt={1}>
          <Box mr={1}>
            <FaIcon icon={faInfoCircle} />
          </Box>
          <Box>{helperText}</Box>
        </FormHelperText>
      )}
      <FormErrorMessage mt={1} lineHeight="normal">
        <Box mr={1}>
          <FaIcon icon={faExclamationCircle} />
        </Box>
        <Box>{errorMessage}</Box>
      </FormErrorMessage>
    </FormControl>
  );
};

export default FormField;