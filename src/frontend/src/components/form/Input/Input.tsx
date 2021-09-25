import { forwardRef, ReactNode } from 'react';
import {
  Input as ChakraInput,
  InputGroup,
  InputLeftAddon,
  InputLeftElement,
  InputRightAddon,
  InputRightElement,
  InputProps as ChakraInputProps,
  InputElementProps,
} from '@chakra-ui/react';

const addonProps = {
  px: 3,
  border: '2px solid',
  borderColor: 'gray.300',
};

export interface InputProps extends ChakraInputProps {
  leftAddon?: ReactNode,
  leftElement?: ReactNode,
  leftElementProps?: InputElementProps,
  rightAddon?: ReactNode,
  rightElement?: ReactNode,
  rightElementProps?: InputElementProps,
}

const Input = forwardRef<HTMLInputElement, InputProps>((props, ref) => {
  const {
    leftAddon,
    leftElement,
    leftElementProps,
    rightAddon,
    rightElement,
    rightElementProps,
    ...InputProps
  } = props;

  return (
    <InputGroup>
      {leftAddon && (
        <InputLeftAddon {...addonProps}>{leftAddon}</InputLeftAddon>
      )}
      {leftElement && (
        <InputLeftElement left="2px" {...leftElementProps}>{leftElement}</InputLeftElement>
      )}
      <ChakraInput
        px={3}
        borderWidth="2px"
        borderColor="gray.300"
        _hover={{
          borderColor: 'gray.400',
        }}
        _focus={{
          borderColor: 'brand.500',
        }}
        _invalid={{
          borderColor: 'red.500',
          boxShadow: false,
        }}
        ref={ref}
        {...InputProps}
      />
      {rightElement && (
        <InputRightElement right="2px" {...rightElementProps}>{rightElement}</InputRightElement>
      )}
      {rightAddon && (
        <InputRightAddon {...addonProps}>{rightAddon}</InputRightAddon>
      )}
    </InputGroup>
  );
});

export default Input;