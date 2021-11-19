import { UserDto } from 'dto/user';

export interface CreateReservationDto {
  quantity: number,
}

export interface ReservationDto {
  id: number,
  createdAt: string,
  completedAt: string | null,
  quantity: number,
  user: UserDto,
}

export interface IReservationPrompt {
  isOpen: boolean,
  onClose: () => void,
  quantity: number,
}