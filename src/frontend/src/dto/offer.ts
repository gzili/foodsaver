export interface IOfferDto {
  id: number,
  quantity: number,
  availableQuantity: number,
  description: string,
  createdAt: string,
  expiresAt: string,
  giver: {
    id: number,
    username: string,
    avatarPath: string | null,
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
    minQuantity: number,
  },
}