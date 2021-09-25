import { Button, HStack } from '@chakra-ui/react';

import { IStep, PublicProfileData } from '../interfaces';
import { FlowContainer, FlowContent, FlowHeader, BottomBar, ProgressIndicator } from '../layout';

export default function PublicProfileFlow(props: IStep<PublicProfileData>) {
  const {
    currentStep,
    stepCount,
    onPrev,
    onNext
  } = props;

  return (
    <FlowContainer>
      <FlowHeader title="Public Profile" description="This information is displayed alongside the offers you create" />
      <FlowContent>
        Form Fields
      </FlowContent>
      <BottomBar>
        <ProgressIndicator count={stepCount} activeIndex={currentStep} />
        <HStack spacing={2}>
          <Button onClick={onPrev}>Back</Button>
          <Button colorScheme="brand" onClick={() => onNext({})}>Next</Button>
        </HStack>
      </BottomBar>
    </FlowContainer>
  );
}