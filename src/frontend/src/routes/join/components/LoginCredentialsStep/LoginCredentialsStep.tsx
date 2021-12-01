import { Alert, AlertIcon, AlertDescription, Button, HStack, VStack } from '@chakra-ui/react';
import * as yup from 'yup';
import { yupResolver } from '@hookform/resolvers/yup';
import { Resolver, useForm } from 'react-hook-form';
import { useMutation } from 'react-query';
import { HTTPError } from 'ky';
import { useHistory } from 'react-router';
import api from 'contexts/apiContext';

import { ICreateUserDto } from 'dto/user';
import { FieldWithController, Input } from 'components/form';

import { IStep, LoginCredentialsData } from '../interfaces';
import { StepContainer, StepContent, StepHeader, BottomBar, ProgressIndicator } from '../layout';
import { useState } from 'react';

const publicProfileSchema = yup.object().shape({
  email: yup.string().email('Please provide a valid email').required('Please provide your email'),
  password: yup.string().required('Please choose a password'),
  confirmPassword: yup.string().oneOf([yup.ref('password')], 'Passwords must match'),
});

const resolver: Resolver<LoginCredentialsData> = yupResolver(publicProfileSchema) as any;

function registerUser(data: ICreateUserDto) {
  const fd = new FormData();

  fd.append('userType', data.userType.toString());
  fd.append('username', data.username);
  data.avatar && fd.append('avatar', data.avatar);
  fd.append('addressStreet', data.address.street);
  fd.append('addressCity', data.address.city);
  fd.append('email', data.email);
  fd.append('password', data.password);

  return api.post('users/register', { body: fd });
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
    const { avatar, street, city, ...restData } = data as Required<typeof data>;

    mutate({
      avatar: avatar[0],
      address: {
        street,
        city,
      },
      ...restData,
      ...values,
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