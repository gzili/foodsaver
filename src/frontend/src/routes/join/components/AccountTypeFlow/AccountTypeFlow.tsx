import { Box, Button } from '@chakra-ui/react';

import { faHandHoldingHeart, FaIcon, faStore, faUser } from 'components/core';
import { useState } from 'react';

import { IAccountFlow, AccountType } from '../interfaces';
import { FlowContainer, FlowContent, FlowHeader, BottomBar, ProgressDots } from '../layout';
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

export default function AccountTypeFlow(props: IAccountFlow<AccountType>) {
  const {
    onNext
  } = props;

  const [accountType, setAccountType] = useState(0);

  const handleSubmit = () => {
    onNext(accountType);
  };

  return (
    <FlowContainer>
      <FlowHeader title="Account Type" description="Choose your account type for optimal experience" />
      <FlowContent>
        <AccountTypeRadioGroup options={accountTypeOptions} value={accountType} onChange={v => setAccountType(v)} />
      </FlowContent>
      <BottomBar>
        <ProgressDots count={3} activeIndex={0} />
        <Box>
          <Button colorScheme="brand" onClick={handleSubmit}>Next</Button>
        </Box>
      </BottomBar>
    </FlowContainer>
  );
}