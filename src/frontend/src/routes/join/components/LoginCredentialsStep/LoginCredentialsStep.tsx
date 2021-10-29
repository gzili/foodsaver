import { Alert, AlertIcon, AlertDescription, Button, HStack, VStack } from '@chakra-ui/react';
import * as yup from 'yup';
import { yupResolver } from '@hookform/resolvers/yup';
import { Resolver, useForm } from 'react-hook-form';
import { useMutation } from 'react-query';
import { HTTPError } from 'ky';
import { useHistory } from 'react-router';
import api from 'contexts/api.context';

import { ICreateUserDto } from 'dto/user';
import { FieldWithController, Input } from 'components/form';

import { IStep, LoginCredentialsData } from '../interfaces';
import { StepContainer, StepContent, StepHeader, BottomBar, ProgressIndicator } from '../layout';
import { useState } from 'react';

const publicProfileSchema = yup.object().shape({
  email: yup.string().email('Please provide a valid email').required(),
  password: yup.string().min(6, 'Password must be at least 6 characters long'),
  confirmPassword: yup.string().oneOf([yup.ref('password')], 'Passwords must match'),
});

const resolver: Resolver<LoginCredentialsData> = yupResolver(publicProfileSchema) as any;

function registerUser(data: ICreateUserDto) {
  return api.post('user/register', { json: data }).json<any>();
}

export default function PublicProfileFlow(props: IStep<LoginCredentialsData>) {
  const {
    data,
    currentStep,
    stepCount,
    onPrev,
  } = props;

  const { control, getValues, handleSubmit } = useForm<LoginCredentialsData>({
    defaultValues: {
      email: data.email ?? '',
      password: data.password ?? '',
      confirmPassword: data.confirmPassword ?? '',
    },
    resolver,
  });

  const history = useHistory();
  const [error, setError] = useState('');

  const { mutate, isLoading, isError } = useMutation<any, HTTPError, any>(registerUser, {
    onSuccess: () => {
      history.replace('/login');
    },
    onError: async (e: HTTPError) => {
      if (e.response) {
        const message = await e.response.json();
        console.log(message);
        setError(message?.title ?? String(message));
      } else {
        setError(e.message);
      }
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
  }

  return (
    <StepContainer>
      <StepHeader
        title="Login credentials"
        description="Please provide the login credentials you will use to access your account"
      />
      <StepContent>
        {isError && (
          <Alert status="error" mb={2} borderRadius="md">
            <AlertIcon />
            <AlertDescription>{error}</AlertDescription>
          </Alert>
        )}
        <form id="login-credentials-form" onSubmit={handleSubmit(handleNext)}>
          <VStack spacing={4}>
            <FieldWithController control={control} name="email" label="Email">
              {field => <Input {...field} type="email" />}
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