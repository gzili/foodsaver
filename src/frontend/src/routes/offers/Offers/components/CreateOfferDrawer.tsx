import { Button, Modal, ModalOverlay, ModalBody, ModalCloseButton, ModalHeader, ModalContent, ModalFooter, Select, Textarea, VStack } from '@chakra-ui/react';
import * as yup from 'yup';
import { yupResolver } from '@hookform/resolvers/yup';
import { useForm } from 'react-hook-form';
import { DatePickerInput, FieldWithController, FilePicker, Input } from 'components';
import api from 'contexts/apiContext';
import { useMutation, useQueryClient } from 'react-query';
import { endOfDay } from 'date-fns';

interface ICreateOfferDrawer {
  isOpen: boolean,
  onClose: () => void,
}

interface FormValues {
  foodName: string,
  foodPhoto: File[] | null,
  foodUnit: string,
  offerQuantity: number | string,
  offerDescription: string,
  offerExpirationDate: Date | null,
}

const validationSchema = yup.object({
  foodName: yup.string().required("This field is required"),
  foodPhoto: yup.array().length(1, "Please upload a photo"),
  foodUnit: yup.string().required("Please choose a unit"),
  offerQuantity: yup.number().typeError("Please provide the quantity"),
  offerExpirationDate: yup.date().typeError("Please set an expiration date"),
  offerDescription: yup.string(),
});

const resolver = yupResolver(validationSchema) as any;

function createOffer(data: FormData) {
  return api.post('offers', { body: data });
}

function CreateOfferContent({ onClose }: { onClose: () => void }) {
  const { control, handleSubmit: onSubmit } = useForm<FormValues>({
    resolver,
    defaultValues: {
      foodName: '',
      foodPhoto: [],
      offerQuantity: '',
      foodUnit: 'pcs',
      offerExpirationDate: null,
      offerDescription: '',
    }
  });

  const queryClient = useQueryClient();
  const { mutate, isLoading } = useMutation(createOffer, {
    onSuccess: () => {
      queryClient.invalidateQueries('offers');
      onClose();
    },
    onError: (error: Error) => {
      console.log(error);
    },
  });

  const handleSubmit = (data: FormValues) => {
    const formData = new FormData();

    formData.append('foodName', data.foodName);
    formData.append('foodPhoto', data.foodPhoto![0]);
    formData.append('foodUnit', data.foodUnit);
    formData.append('quantity', data.offerQuantity.toString());
    formData.append('expiresAt', endOfDay(data.offerExpirationDate!).toJSON());
    formData.append('description', data.offerDescription);

    mutate(formData);
  };

  return (
    <>
      <ModalBody overflowX="hidden">
        <form id="create-offer-form" onSubmit={onSubmit(handleSubmit)}>
          <VStack spacing={2}>
            <FieldWithController control={control} name="foodName" label="Title">
              {props => <Input {...props} />}
            </FieldWithController>
            <FieldWithController control={control} name="foodPhoto" label="Photo">
              {(props, state) => <FilePicker {...props} {...state} accept="image/*" />}
            </FieldWithController>
            <FieldWithController control={control} name="offerQuantity" label="Quantity">
              {props => <Input {...props} />}
            </FieldWithController>
            <FieldWithController control={control} name="foodUnit" label="Measurement unit">
              {(props) => (
                <Select
                  {...props}
                  borderWidth="2px"
                  borderColor="gray.300"
                  _hover={{
                    borderColor: 'gray.400',
                  }}
                  _focus={{
                    borderColor: 'brand.500',
                  }}
                >
                  <option value="g">g (grams)</option>
                  <option value="kg">kg (kilograms)</option>
                  <option value="l">l (litres)</option>
                  <option value="ml">ml (mililitres)</option>
                  <option value="pcs">pcs (pieces)</option>
                </Select>
              )}
            </FieldWithController>
            <FieldWithController control={control} name="offerExpirationDate" label="Expiration date">
              {(props) => <DatePickerInput {...props} disablePast weekStartsOnMonday />}
            </FieldWithController>
            <FieldWithController control={control} name="offerDescription" label="Description">
              {props => (
                <Textarea
                  {...props}
                  resize="none"
                  px={3}
                  border="2px solid"
                  borderColor="gray.300"
                  _hover={{
                    borderColor: 'gray.400',
                  }}
                  _focus={{
                    borderColor: 'brand.500',
                  }}
                />
              )}
            </FieldWithController>
          </VStack>
        </form>
      </ModalBody>
      <ModalFooter>
        <Button isDisabled={isLoading} onClick={onClose} mr={2}>Cancel</Button>
        <Button colorScheme="brand" type="submit" form="create-offer-form" isLoading={isLoading}>Create</Button>
      </ModalFooter>
    </>
  );
}

export function CreateOfferDrawer(props: ICreateOfferDrawer) {
  const {
    isOpen,
    onClose,
  } = props;

  return (
    <Modal isOpen={isOpen} onClose={onClose} scrollBehavior="inside" size="full" isCentered>
      <ModalOverlay />
      <ModalContent maxH="90%" w="90%" sx={{ minH: false }}>
        <ModalCloseButton />
        <ModalHeader>Create offer</ModalHeader>
        <CreateOfferContent onClose={onClose} />
      </ModalContent>
    </Modal>
  );
} 