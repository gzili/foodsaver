import { Button, HStack, VStack } from '@chakra-ui/react';
import * as yup from 'yup';
import { yupResolver } from '@hookform/resolvers/yup';
import { Resolver, useForm } from 'react-hook-form';

import { FieldWithController, Input } from 'components/form';

import { IStep, PublicProfileData } from '../interfaces';
import { StepContainer, StepContent, StepHeader, BottomBar, ProgressIndicator } from '../layout';

const publicProfileSchema = yup.object().shape({
  username: yup.string().required('Please choose a display name'),
  street: yup.string().required('Please provide your street address'),
  city: yup.string().required('Please provide the city / region'),
});

const resolver: Resolver<PublicProfileData> = yupResolver(publicProfileSchema) as any;

export default function PublicProfileFlow(props: IStep<PublicProfileData>) {
  const {
    data,
    currentStep,
    stepCount,
    onPrev,
    onNext
  } = props;

  const { control, getValues, handleSubmit } = useForm<PublicProfileData>({
    defaultValues: {
      username: data.username ?? '',
      street: data.street ?? '',
      city: data.city ?? '',
    },
    resolver,
  });

  const handleNext = (data: PublicProfileData) => {
    onNext(data);
  }

  return (
    <StepContainer>
      <StepHeader title="Public Profile" description="This information is displayed alongside the offers you create" />
      <StepContent>
        <form id="public-profile-form" onSubmit={handleSubmit(handleNext)}>
          <VStack spacing={4}>
            <FieldWithController control={control} name="username" label="Display name">
              {field => <Input {...field} />}
            </FieldWithController>
            <FieldWithController control={control} name="street" label="Street address">
              {field => <Input {...field} />}
            </FieldWithController>
            <FieldWithController control={control} name="city" label="City / county">
              {field => <Input {...field} />}
            </FieldWithController>
          </VStack>
        </form>
      </StepContent>
      <BottomBar>
        <ProgressIndicator count={stepCount} activeIndex={currentStep} />
        <HStack spacing={2}>
          <Button onClick={() => onPrev(getValues())}>Back</Button>
          <Button type="submit" form="public-profile-form" colorScheme="brand">Next</Button>
        </HStack>
      </BottomBar>
    </StepContainer>
  );
}