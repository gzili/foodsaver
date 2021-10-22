export interface IOfferDto {
  id: number,
  quantity: number,
  description: string,
  creationDate: string,
  expirationDate: string,
  giver: {
    name: string,
    avatar: string,
    address: {
      streetAddress: string,
      city: string,
    }
  },
  food: {
    id: number,
    name: string,
    imagePath: string,
    unit: string,
  },
}