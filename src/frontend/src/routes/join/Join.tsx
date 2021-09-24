import { Box, Button, Flex, Heading, HStack, useRadio, UseRadioProps, useRadioGroup, VStack } from '@chakra-ui/react';

import { faCircle, faHandHoldingHeart, FaIcon, faStore, faUser } from 'components/core';

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
        h="100px"
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
            <Box fontSize="lg" fontWeight="bold">{title}</Box>
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
    title: 'Enterprise',
    description: 'For representatives of bussineses, such as restaurants or shops',
    value: '1'
  },
  {
    icon: <FaIcon icon={faHandHoldingHeart} fixedWidth />,
    title: 'Non-profit',
    description: 'For representatives of non-profit organizations',
    value: '2'
  }
];

function AccountTypeForm() {
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

interface IProgressDots {
  count: number,
  activeIndex: number
}

function ProgressDots(props: IProgressDots) {
  const {
    count,
    activeIndex
  } = props;

  const dotComponents = [];

  for (let i = 0; i < count; ++i) {
    dotComponents[i] = (
      <Box as={FaIcon} key={i} icon={faCircle} fontSize="xs" color={i <= activeIndex ? 'brand.500' : undefined} />
    );
  }

  return (
    <HStack spacing={4}>
      <HStack spacing={2}>
        {dotComponents}
      </HStack>
      <Box>Step {activeIndex + 1} of {count}</Box>
    </HStack>
  );
}

function Join() {
  return (
    <Box px={8} pt={6} pb={12} borderTop="1px solid" borderColor="gray.100">
      <Box mb={4}>
        <Heading as="h1">Account Type</Heading>
        <Box mt={1} lineHeight="1.25" color="gray.500">Choose your account type for optimal experience</Box>
      </Box>
      <Box>
        <AccountTypeForm />
      </Box>
      <Flex
        pos="fixed"
        bottom={0}
        left={0}
        w="100%"
        h={14}
        align="center"
        justify="space-between"
        px={4}
      >
        <ProgressDots count={3} activeIndex={0} />
        <Box>
          <Button h={8} colorScheme="brand">Next</Button>
        </Box>
      </Flex>
    </Box>
  );
}

export default Join;