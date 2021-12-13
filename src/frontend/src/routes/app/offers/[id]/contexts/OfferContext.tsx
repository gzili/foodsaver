import type { IOfferDto } from 'dto/offer';
import { createContext, ReactNode, useContext } from 'react';

const offerContext = createContext<IOfferDto>(null!);

export const useOffer = () => useContext(offerContext);

interface IOfferProvider {
  value: IOfferDto,
  children: ReactNode
}

export function OfferProvider(props: IOfferProvider) {
  const { value, children } = props;

  return (
    <offerContext.Provider value={value}>
      {children}
    </offerContext.Provider>
  );
}