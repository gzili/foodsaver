export enum UserType {
  Individual,
  Business,
  Charity
}

export interface ICreateUserDto {
  userType: UserType,
  username: string,
  address: {
    street: string,
    city: string,
  },
  email: string,
  password: string
}