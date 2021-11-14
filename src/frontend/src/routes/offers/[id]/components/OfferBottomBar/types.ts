export interface ReservationDto {
  quantity: number,
}

export interface IReservationPrompt {
  isOpen: boolean,
  onClose: () => void,
  quantity: number,
}