import { Box, Button } from '@chakra-ui/react';

import { faHandHoldingHeart, FaIcon, faStore, faUser } from 'components/core';
import { useState } from 'react';

import { IStep, AccountTypeData } from '../interfaces';
import { FlowContainer, FlowContent, FlowHeader, BottomBar, ProgressIndicator } from '../layout';
import AccountTypeRadioGroup, { AccountTypeOption } from './components/AccountTypeRadioGroup';

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

export default function AccountTypeFlow(props: IStep<AccountTypeData>) {
  const {
    currentStep,
    stepCount,
    data,
    onNext
  } = props;

  const [accountType, setAccountType] = useState(data.accountType ?? 0);

  const handleSubmit = () => {
    onNext({ accountType });
  };

  return (
    <FlowContainer>
      <FlowHeader title="Account Type" description="Choose your account type for optimal experience" />
      <FlowContent>
        <AccountTypeRadioGroup options={accountTypeOptions} value={accountType} onChange={v => setAccountType(v)} />
      </FlowContent>
      <BottomBar>
        <ProgressIndicator count={stepCount} activeIndex={currentStep} />
        <Box>
          <Button colorScheme="brand" onClick={handleSubmit}>Next</Button>
        </Box>
      </BottomBar>
    </FlowContainer>
  );
}