import { forwardRef, RefCallback, useEffect, useMemo, useState } from 'react';
import { useCombobox } from 'downshift';
import { usePopper } from 'react-popper';
import { Box, Spinner, Tooltip, useFormControlProps } from '@chakra-ui/react';

import { useAsyncOrLocalData } from 'hooks';

import { FaIcon, faExclamationTriangle } from 'components/core';
import Input, { InputProps } from '../Input';

interface TextAutocompleteProps extends Omit<InputProps, 'value' | 'onChange'> {
  items: string[] | (() => Promise<string[]>),
  value: string,
  onChange: (value: string) => void,
}

interface StatusIndicatorProps {
  isLoading: boolean,
  isError: boolean,
}

const StatusIndicator = (props: StatusIndicatorProps) => {
  const {
    isLoading,
    isError,
  } = props;

  if (isLoading) {
    return (
      <Spinner size="sm" color="gray.600" />
    );
  }

  if (isError) {
    return (
      <Tooltip label="Failed to load options">
        <Box color="red.500" fontSize="sm">
          <FaIcon icon={faExclamationTriangle} />
        </Box>
      </Tooltip>
    );
  }

  return null;
};

const TextAutocomplete = forwardRef<HTMLInputElement, TextAutocompleteProps>((props, ref) => {
  const {
    items: itemsProp,
    value,
    onChange,
    ...InputProps
  } = props;

  const { getData, isLoading, isError, data: items } = useAsyncOrLocalData(itemsProp);

  const inputItems = useMemo(() => {
    if (value !== '' && items) {
      return items.filter(item => item.toLowerCase().startsWith(value.toLowerCase()));
    } else {
      return [];
    }
  }, [value, items]);

  const { id, isDisabled } = useFormControlProps(InputProps);

  const {
    isOpen,
    getMenuProps,
    getInputProps,
    getComboboxProps,
    getItemProps,
  } = useCombobox({
    defaultHighlightedIndex: 0,
    inputId: id,
    items: inputItems,
    onInputValueChange: ({ inputValue }) => {
      onChange(inputValue!);
    },
  });

  const handleInputFocus = () => {
    if (!items && !isLoading) getData();
  };

  const [referenceEl, setReferenceEl]: [HTMLInputElement | null, RefCallback<HTMLInputElement>] = useState<HTMLInputElement | null>(null);
  const [popperEl, setPopperEl]: [HTMLDivElement | null, RefCallback<HTMLDivElement>] = useState<HTMLDivElement | null>(null);

  const modifiers = useMemo(() => ({
    modifiers: [
      {
        name: 'offset',
        options: {
          offset: [0, 8],
        },
      },
      {
        name: 'eventListeners',
        enabled: isOpen,
      },
    ],
  }), [isOpen]);

  const { styles: popperStyles, attributes: popperProps, forceUpdate } = usePopper(referenceEl, popperEl, modifiers);

  useEffect(() => {
    forceUpdate?.();
  }, [inputItems, forceUpdate]);

  return (
    <Box pos="relative">
      <Box {...getComboboxProps({ ref: setReferenceEl })}>
        <Input
          {...InputProps}
          {...getInputProps({ disabled: isDisabled, ref, onFocus: handleInputFocus })}
          rightElement={<StatusIndicator isLoading={isLoading} isError={isError} />}
        />
      </Box>
      <Box
        zIndex="popover"
        w="100%"
        maxH="200px"
        overflowY="auto"
        bg="white"
        borderRadius="md"
        boxShadow="popover"
        style={popperStyles.popper}
        {...popperProps.popper}
        {...getMenuProps({ ref: setPopperEl })}
      >
        {isOpen && !isLoading && (inputItems.length > 0) && (
          <Box py={1}>
            {inputItems.map((item, index) => (
              <Box
                key={item}
                px={4}
                py={1}
                cursor="pointer"
                _selected={{
                  bg: 'brand.50'
                }}
                {...getItemProps({ item, index })}
                children={item}
              />
            ))}
          </Box>
        )}
      </Box>
    </Box>
  );
});

export default TextAutocomplete;