import { Button, Drawer, DrawerOverlay, DrawerBody, DrawerCloseButton, DrawerHeader, DrawerContent, DrawerFooter, Textarea, VStack } from '@chakra-ui/react';
import * as yup from 'yup';
import { yupResolver } from '@hookform/resolvers/yup';
import { useForm } from 'react-hook-form';
import { DatePickerInput, FieldWithController, FilePicker, Input, TextAutocomplete } from 'components';

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
  offerDescription: yup.string(),
  offerExpirationDate: yup.date().nullable(),
});

const resolver = yupResolver(validationSchema) as any;

const units = [
  'kg',
  'g',
  'l',
  'ml',
  'pc',
  'pcs',
].sort();

function CreateOfferContent() {
  const { control, handleSubmit: onSubmit } = useForm<FormValues>({
    resolver,
    defaultValues: {
      foodName: '',
      foodPhoto: [],
      offerQuantity: '',
      foodUnit: '',
      offerExpirationDate: null,
      offerDescription: '',
    }
  });

  const handleSubmit = (data: FormValues) => {
    console.log(data);
  };

  return (
    <>
      <DrawerBody>
        <form id="create-offer-form" onSubmit={onSubmit(handleSubmit)}>
          <VStack spacing={2}>
            <FieldWithController control={control} name="foodName" label="Food">
              {props => <Input {...props} />}
            </FieldWithController>
            <FieldWithController control={control} name="foodPhoto" label="Photo">
              {(props, state) => <FilePicker {...props} {...state} />}
            </FieldWithController>
            <FieldWithController control={control} name="offerQuantity" label="Quantity">
              {props => <Input {...props} />}
            </FieldWithController>
            <FieldWithController control={control} name="foodUnit" label="Measurement unit">
              {(props) => <TextAutocomplete items={units} {...props} />}
            </FieldWithController>
            <FieldWithController control={control} name="offerExpirationDate" label="Expiration date">
              {(props) => <DatePickerInput {...props} disablePast weekStartsOnMonday />}
            </FieldWithController>
            <FieldWithController control={control} name="offerDescription" label="Description" optional>
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
      </DrawerBody>
      <DrawerFooter>
        <Button colorScheme="brand" type="submit" form="create-offer-form">Create</Button>
      </DrawerFooter>
    </>
  );
}

export function CreateOfferDrawer(props: ICreateOfferDrawer) {
  const {
    isOpen,
    onClose,
  } = props;

  return (
    <Drawer isOpen={isOpen} onClose={onClose} size="full" placement="bottom">
      <DrawerOverlay />
      <DrawerContent>
        <DrawerCloseButton />
        <DrawerHeader>Create offer</DrawerHeader>
        <CreateOfferContent />
      </DrawerContent>
    </Drawer>
  );
} 