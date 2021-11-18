export enum UserType {
  Individual,
  Business,
  Charity
}

export interface UserDto {
  id: number,
  userType: UserType,
  username: string,
  avatarPath: string,
  address: {
    street: string,
    city: string,
  },
  email: string,
}

export interface ICreateUserDto extends Omit<UserDto, 'id' | 'avatarPath'> {
  password: string,
}