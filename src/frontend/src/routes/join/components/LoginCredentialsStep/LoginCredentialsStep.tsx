import { Button, HStack } from '@chakra-ui/react';

import { IStep, LoginCredentialsData } from '../interfaces';
import { StepContainer, StepContent, StepHeader, BottomBar, ProgressIndicator } from '../layout';

export default function PublicProfileFlow(props: IStep<LoginCredentialsData>) {
  const {
    currentStep,
    stepCount,
    onPrev,
  } = props;

  return (
    <StepContainer>
      <StepHeader title="Login credentials" description="Please provide the login credentials you will use to access your account" />
      <StepContent>
        Form Fields
      </StepContent>
      <BottomBar>
        <ProgressIndicator count={stepCount} activeIndex={currentStep} />
        <HStack spacing={2}>
          <Button onClick={onPrev}>Back</Button>
          <Button colorScheme="brand">Next</Button>
        </HStack>
      </BottomBar>
    </StepContainer>
  );
}