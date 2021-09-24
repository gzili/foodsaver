import { Box, Flex, useRadio, UseRadioProps, useRadioGroup, VStack } from '@chakra-ui/react';

import { faHandHoldingHeart, FaIcon, faStore, faUser } from 'components/core';

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
        {...getCheckboxProps()}
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

interface AccountTypeOption {
  icon: JSX.Element,
  title: string,
  description: string,
  value: string
}

const accountTypeOptions: AccountTypeOption[] = [
  {
    icon: <FaIcon icon={faUser} fixedWidth />,
    title: 'Individual',
    description: 'For individuals offering their own food',
    value: '0'
  },
  {
    icon: <FaIcon icon={faStore} fixedWidth />,
    title: 'Business',
    description: 'For representatives of businesses, such as restaurants or shops',
    value: '1'
  },
  {
    icon: <FaIcon icon={faHandHoldingHeart} fixedWidth />,
    title: 'Non-profit',
    description: 'For representatives of non-profit organizations',
    value: '2'
  }
];

export default function AccountTypeForm() {
  const { getRootProps, getRadioProps } = useRadioGroup({
    name: 'accountType',
    defaultValue: '0',
    onChange: console.log
  });

  return (
    <VStack spacing={2} {...getRootProps()}>
      {accountTypeOptions.map(({ value, ...cardProps }) => (
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