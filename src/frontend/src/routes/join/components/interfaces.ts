export interface IStep<T> {
  data: any,
  currentStep: number,
  stepCount: number,
  onPrev?: () => void,
  onNext: (data: T) => void,
}

export enum AccountType {
  Individual,
  Business,
  Nonprofit
}

export interface AccountTypeData {
  accountType: AccountType
}

export interface PublicProfileData {}

export interface LoginCredentialsData {}