export enum UserType {
  Individual,
  Business,
  Nonprofit
}

export interface ICreateUserDto {
  userType: UserType,
  name: string,
  address: {
    streetAddress: string,
    city: string,
  },
  email: string,
  password: string
}