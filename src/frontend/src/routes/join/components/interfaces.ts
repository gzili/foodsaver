import { UserType } from 'dto/user';

export interface UserTypeData {
  userType: UserType
}

export interface PublicProfileData {
  username: string,
  avatar: File[],
  street: string,
  city: string,
}

export interface LoginCredentialsData {
  email: string,
  password: string,
  confirmPassword: string,
}

export type FormData = UserTypeData & PublicProfileData & LoginCredentialsData;

export interface IStep<T> {
  data: Partial<FormData>,
  currentStep: number,
  stepCount: number,
  onPrev: (data: T) => void,
  onNext: (data: T) => void,
}