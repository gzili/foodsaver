import { Button, HStack, useToast, VStack } from '@chakra-ui/react';
import * as yup from 'yup';
import { yupResolver } from '@hookform/resolvers/yup';
import { Resolver, useForm } from 'react-hook-form';
import { useMutation } from 'react-query';

import { ICreateUserDto } from 'dto/user';
import { FieldWithController, Input } from 'components/form';

import { IStep, LoginCredentialsData } from '../interfaces';
import { StepContainer, StepContent, StepHeader, BottomBar, ProgressIndicator } from '../layout';

const publicProfileSchema = yup.object().shape({
  email: yup.string().email('Please provide a valid email').required(),
  password: yup.string().min(6, 'Password must be at least 6 characters long'),
  confirmPassword: yup.string().oneOf([yup.ref('password')], 'Passwords must match'),
});

const resolver: Resolver<LoginCredentialsData> = yupResolver(publicProfileSchema) as any;

async function registerUser(data: ICreateUserDto) {
  let res;

  try {
    res = await fetch('api/users/register', {
      method: 'POST',
      body: JSON.stringify(data),
      headers: {
        'Content-Type': 'application/json',
      },
    });
  } catch (err) {
    throw new Error(`Server is unreachable`);
  }

  if (!res.ok) {
    throw new Error(`Server responded with status code ${res.status} ${res.statusText}`);
  }

  return await res.json();
}

export default function PublicProfileFlow(props: IStep<LoginCredentialsData>) {
  const {
    data,
    currentStep,
    stepCount,
    onPrev,
    // onNext
  } = props;

  const { control, getValues, handleSubmit } = useForm<LoginCredentialsData>({
    defaultValues: {
      email: data.email ?? '',
      password: data.password ?? '',
      confirmPassword: data.confirmPassword ?? '',
    },
    resolver,
  });

  const toast = useToast();

  const { mutate, isLoading } = useMutation(registerUser, {
    onSuccess: (data: any) => {
      console.log(data);
      toast({
        title: 'Account created successfully',
        status: 'success',
        position: 'top',
        duration: 3000,
      });
    },
    onError: (e: Error) => {
      toast({
        title: 'Failed to create account',
        description: e.message,
        status: 'error',
        position: 'top',
        duration: 3000,
        isClosable: true
      });
    },
  });

  const handleNext = (values: LoginCredentialsData) => {
    const {
      userType: accountType,
      name,
      street,
      city
    } = data as Required<typeof data>;

    const {
      email,
      password
    } = values;

    mutate({
      userType: accountType,
      name,
      address: {
        streetAddress: street,
        city,
      },
      email,
      password
    });

    // onNext(data);
  }

  return (
    <StepContainer>
      <StepHeader
        title="Login credentials"
        description="Please provide the login credentials you will use to access your account"
      />
      <StepContent>
        <form id="login-credentials-form" onSubmit={handleSubmit(handleNext)}>
          <VStack spacing={4}>
            <FieldWithController control={control} name="email" label="Email">
              {field => <Input {...field} />}
            </FieldWithController>
            <FieldWithController control={control} name="password" label="Password">
              {field => <Input {...field} type="password" />}
            </FieldWithController>
            <FieldWithController control={control} name="confirmPassword" label="Confirm password">
              {field => <Input {...field} type="password" />}
            </FieldWithController>
          </VStack>
        </form>
      </StepContent>
      <BottomBar>
        <ProgressIndicator count={stepCount} activeIndex={currentStep} />
        <HStack spacing={2}>
          <Button isDisabled={isLoading} onClick={() => onPrev(getValues())}>Back</Button>
          <Button isLoading={isLoading} type="submit" form="login-credentials-form" colorScheme="brand">Finish</Button>
        </HStack>
      </BottomBar>
    </StepContainer>
  );
}