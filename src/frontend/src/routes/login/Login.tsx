import { Box, Button, Heading, VStack } from '@chakra-ui/react';
import * as yup from 'yup';
import { yupResolver } from '@hookform/resolvers/yup';
import { useForm } from 'react-hook-form';
import { FieldWithController, Input } from 'components';

interface FormValues {
  email: string,
  password: string,
}

const validationSchema = yup.object({
  email: yup.string().email('Whoops! This does not look like a valid email').required('Please enter your email'),
  password: yup.string().required('Please enter your password'),
});

const resolver = yupResolver(validationSchema) as any;

const defaultValues = {
  email: '',
  password: '',
}

function Login() {
  const { control, handleSubmit } = useForm<FormValues>({
    defaultValues,
    resolver,
  });

  const handleLogin = (data: FormValues) => {
    console.log(data);
  };

  return (
    <Box maxW="280px" m="0 auto" py="4">
      <Box textAlign="center" fontSize="xl" fontWeight="bold" mb="4">food<Box as="span" color="brand.500">saver</Box></Box>
      <Box>
        <Heading as="h1" mb="2" textAlign="center">Log in</Heading>
        <Box>
          <form onSubmit={handleSubmit(handleLogin)}>
            <VStack spacing="2">
              <FieldWithController control={control} name="email" label="Email" optional>
                {(props) => <Input {...props} />}
              </FieldWithController>
              <FieldWithController control={control} name="password" label="Password" optional>
                {(props) => <Input {...props} type="password" />}
              </FieldWithController>
              <Button w="100%" colorScheme="brand" type="submit">Log in</Button>
            </VStack>
          </form>
        </Box>
      </Box>
    </Box>
  );
}

export default Login;