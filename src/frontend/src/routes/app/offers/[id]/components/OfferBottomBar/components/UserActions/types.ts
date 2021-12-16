import { ReservationDto } from '../../types';

export interface ICreatorReservation extends ReservationDto {
  pin: number,
}