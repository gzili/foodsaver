import { Button, HStack } from '@chakra-ui/react';

import { IStep, PublicProfileData } from '../interfaces';
import { StepContainer, StepContent, StepHeader, BottomBar, ProgressIndicator } from '../layout';

export default function PublicProfileFlow(props: IStep<PublicProfileData>) {
  const {
    currentStep,
    stepCount,
    onPrev,
    onNext
  } = props;

  return (
    <StepContainer>
      <StepHeader title="Public Profile" description="This information is displayed alongside the offers you create" />
      <StepContent>
        Form Fields
      </StepContent>
      <BottomBar>
        <ProgressIndicator count={stepCount} activeIndex={currentStep} />
        <HStack spacing={2}>
          <Button onClick={onPrev}>Back</Button>
          <Button colorScheme="brand" onClick={() => onNext({})}>Next</Button>
        </HStack>
      </BottomBar>
    </StepContainer>
  );
}