export interface IOfferDto {
  id: number,
  quantity: number,
  description: string,
  createdAt: string,
  expiresAt: string,
  giver: {
    username: string,
    address: {
      street: string,
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