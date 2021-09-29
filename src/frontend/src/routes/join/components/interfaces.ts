export enum AccountType {
  Individual,
  Business,
  Nonprofit
}

export interface RegisterUserDto {
  userType: AccountType,
  name: string,
  location: string,
  email: string,
  password: string
}

export interface AccountTypeData {
  accountType: AccountType
}

export interface PublicProfileData {
  name: string,
  street: string,
  city: string,
  postcode: string,
}

export interface LoginCredentialsData {
  email: string,
  password: string,
  confirmPassword: string,
}

export type FormData = AccountTypeData & PublicProfileData & LoginCredentialsData;

export interface IStep<T> {
  data: Partial<FormData>,
  currentStep: number,
  stepCount: number,
  onPrev: (data: T) => void,
  onNext: (data: T) => void,
}