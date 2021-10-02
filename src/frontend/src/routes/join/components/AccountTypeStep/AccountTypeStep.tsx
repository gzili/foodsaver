import { Box, Button } from '@chakra-ui/react';

import { faHandHoldingHeart, FaIcon, faStore, faUser } from 'components/core';
import { useState } from 'react';

import { IStep, UserTypeData } from '../interfaces';
import { StepContainer, StepContent, StepHeader, BottomBar, ProgressIndicator } from '../layout';
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

export default function AccountTypeFlow(props: IStep<UserTypeData>) {
  const {
    currentStep,
    stepCount,
    data,
    onNext
  } = props;

  const [userType, setUserType] = useState(data.userType ?? 0);

  const handleSubmit = () => {
    onNext({ userType });
  };

  return (
    <StepContainer>
      <StepHeader title="Account Type" description="Choose your account type for optimal experience" />
      <StepContent>
        <AccountTypeRadioGroup options={accountTypeOptions} value={userType} onChange={v => setUserType(v)} />
      </StepContent>
      <BottomBar>
        <ProgressIndicator count={stepCount} activeIndex={currentStep} />
        <Box>
          <Button colorScheme="brand" onClick={handleSubmit}>Next</Button>
        </Box>
      </BottomBar>
    </StepContainer>
  );
}