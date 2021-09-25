import { Box, Flex, useRadio, UseRadioProps, useRadioGroup, VStack } from '@chakra-ui/react';

export interface AccountTypeOption {
  icon: JSX.Element,
  title: string,
  description: string,
  value: string
}

interface IAccountTypeRadioCard extends UseRadioProps {
  isSelected?: boolean,
  icon: React.ReactNode,
  title: string,
  description: string,
}

function AccountTypeRadioCard(props: IAccountTypeRadioCard) {
  const {
    icon,
    title,
    description,
    ...radioProps
  } = props;

  const { getInputProps, getCheckboxProps } = useRadio(radioProps);

  return (
    <Box as="label" w="100%">
      <input {...getInputProps()} />
      <Flex
        align="center"
        cursor="pointer"
        h="6rem"
        px={4}
        border="1px solid"
        borderColor="gray.300"
        borderRadius="md"
        _hover={{
          borderColor: 'gray.400'
        }}
        _checked={{
          bg: 'brand.50',
          borderColor: 'brand.500',
          boxShadow: theme => `0 0 0 1px ${theme.colors.brand[500]}`
        }}
        {...getCheckboxProps()}
      >
        <Flex align="center">
          <Box fontSize="2xl" mr={4}>{icon}</Box>
          <Box lineHeight="1.25">
            <Box mb={1} fontWeight="bold">{title}</Box>
            <Box fontSize="sm" color="gray.600">{description}</Box>
          </Box>
        </Flex>
      </Flex>
    </Box>
  );
}

interface IAccountTypeRadioGroup {
  options: AccountTypeOption[],
  value: number,
  onChange: (value: number) => void
}

export default function AccountTypeRadioGroup(props: IAccountTypeRadioGroup) {
  const {
    options,
    value,
    onChange
  } = props;

  const handleChange = (value: string) => onChange(+value);

  const { getRootProps, getRadioProps } = useRadioGroup({
    name: 'accountType',
    value: value.toString(10),
    onChange: handleChange
  });

  return (
    <VStack spacing={2} {...getRootProps()}>
      {options.map(({ value, ...cardProps }) => (
        <AccountTypeRadioCard
          key={value}
          {...cardProps}
          {...getRadioProps({ value })}
        />
        )
      )}
    </VStack>
  );
}