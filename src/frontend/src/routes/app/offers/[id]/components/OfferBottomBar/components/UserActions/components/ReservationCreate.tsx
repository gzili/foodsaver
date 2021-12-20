import {
  AlertDialog, AlertDialogBody, AlertDialogContent, AlertDialogFooter, AlertDialogHeader, AlertDialogOverlay,
  Box, Button,
  Flex,
  HStack,
  useDisclosure,
  useNumberInput
} from '@chakra-ui/react';
import { useRef, useState } from 'react';
import { useMutation, useQueryClient } from 'react-query';

import { B, Input } from 'components';
import api from 'contexts/apiContext';

import { useOffer } from 'routes/app/offers/[id]/contexts/OfferContext';
import type { CreateReservationDto, IReservationPrompt } from '../../../types';

function ReservationCreatePrompt(props: IReservationPrompt) {
  const { isOpen, onClose, quantity } = props;

  const offer = useOffer();

  const queryClient = useQueryClient();

  const cancelButtonRef = useRef<HTMLButtonElement>(null);

  const { mutate: reserve, isLoading } = useMutation({
    mutationFn: (data: CreateReservationDto) => {
      return api.post(`offers/${offer.id}/reservation`, { json: data }).json<CreateReservationDto>();
    },
    onSuccess: reservation => {
      queryClient.setQueryData(['reservation', offer.id], reservation);
      onClose();
    },
    onError: err => console.log(err),
  });

  const handleReserve = () => {
    reserve({ quantity });
  };

  return (
    <AlertDialog isOpen={isOpen} onClose={onClose} leastDestructiveRef={cancelButtonRef} isCentered size="xs">
      <AlertDialogOverlay />
      <AlertDialogContent>
        <AlertDialogHeader>Confirm reservation</AlertDialogHeader>
        <AlertDialogBody>Do you want to reserve <B>{quantity} {offer.food.unit}</B> of <B>{offer.food.name}</B>?</AlertDialogBody>
        <AlertDialogFooter>
          <HStack spacing={2}>
            <Button onClick={onClose} isDisabled={isLoading}>Cancel</Button>
            <Button colorScheme="brand" onClick={handleReserve} isLoading={isLoading}>Confirm</Button>
          </HStack>
        </AlertDialogFooter>
      </AlertDialogContent>
    </AlertDialog> 
  );
}

export function ReservationCreate() {
  const { availableQuantity, food } = useOffer();

  const { isOpen: isPromptOpen, onOpen, onClose } = useDisclosure();

  const [inputValue, setInputValue] = useState(food.minQuantity.toString());
  const [value, setValue] = useState(food.minQuantity);

  const handleChange = (str: string) => {
    setInputValue(str);
    setValue(parseFloat(str));
  }

  const {
    getIncrementButtonProps: inc,
    getInputProps: input,
    getDecrementButtonProps: dec,
  } = useNumberInput({
    step: food.minQuantity,
    min: food.minQuantity,
    max: availableQuantity,
    clampValueOnBlur: true,
    value: inputValue,
    onChange: handleChange,
    focusInputOnChange: false,
  });

  return (
    <>
      <ReservationCreatePrompt isOpen={isPromptOpen} onClose={onClose} quantity={value} />
      <Flex justify="space-between" align="center">
        <HStack spacing={1}>
          <Button size="sm" {...dec()}>-</Button>
          <Box w={12}>
            <Input {...input()} size="sm" borderRadius="md" textAlign="center" />
          </Box>
          <Button size="sm" {...inc()}>+</Button>
        </HStack>
        <Box>
          <Button
            colorScheme="brand"
            isDisabled={isNaN(value) || value <= 0 || value > availableQuantity}
            onClick={onOpen}
          >
            Reserve
          </Button>
        </Box>
      </Flex>
    </>
  );
}