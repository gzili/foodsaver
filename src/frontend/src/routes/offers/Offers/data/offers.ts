import bananas from '../images/bananas.jpg';
import cepelinai from '../images/cepelinai.jpg';
import sukutis from '../images/sukutis.jpg';
import elderlyWoman from '../images/elderly_woman.jpg';
import etnoDvarasLogo from '../images/etno_dvaras_logo.png';
import lidlLogo from '../images/lidl_logo.jpg';

export interface OfferItem {
  id: number,
  dateCreated: string,
  user: {
    name: string,
    avatar: string,
  },
  food: {
    name: string,
    photo: string,
  },
  location: {
    street: string,
    city: string,
  }
}

export const offers: OfferItem[] = [
  {
    id: 0,
    dateCreated: '28 Sep, 7:31',
    user: {
      name: 'Bobutė',
      avatar: elderlyWoman,
    },
    food: {
      name: 'Bananai',
      photo: bananas,
    },
    location: {
      street: 'Pelkynų g. 6',
      city: 'Vilnius',
    }
  },
  {
    id: 1,
    dateCreated: '28 Sep, 12:58',
    user: {
      name: 'Etno dvaras',
      avatar: etnoDvarasLogo,
    },
    food: {
      name: 'Cepelinai',
      photo: cepelinai,
    },
    location: {
      street: 'Ukmergės g. 369',
      city: 'Vilnius',
    }
  },
  {
    id: 2,
    dateCreated: '28 Sep, 9:47',
    user: {
      name: 'Lidl',
      avatar: lidlLogo,
    },
    food: {
      name: 'Marcipaninis sukutis',
      photo: sukutis,
    },
    location: {
      street: 'Vytauto g. 111A',
      city: 'Ukmergė',
    }
  },
];