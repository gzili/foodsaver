import { faHandHoldingHeart, faStore, faUser } from 'components';
import { UserType } from 'dto/user';

export function getUserTypeString(userType: UserType) {
  switch (userType) {
    case UserType.Business:
      return "Business";
    case UserType.Charity:
      return "Charity";
    case UserType.Individual:
      return "Individual";
  }
}

export function getUserTypeFaIcon(userType: UserType) {
  switch (userType) {
    case UserType.Business:
      return faStore;
    case UserType.Charity:
      return faHandHoldingHeart;
    case UserType.Individual:
      return faUser;
  }
}