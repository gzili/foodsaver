export interface IAccountFlow<T> {
  onPrev?: () => void,
  onNext: (data: T) => void,
}

export enum AccountType {
  Individual,
  Business,
  Nonprofit
}