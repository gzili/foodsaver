import { UserDto } from 'dto/user';
import { createContext, ReactNode, useContext, useState } from 'react';
import { useMutation, useQuery } from 'react-query';

import api from './apiContext';

export enum UserType {
  Individual,
  Business,
  Charity
}

export interface LoginData {
  email: string,
  password: string,
}

interface AuthContextValue {
  user: UserDto | null,
  setUser: (user: UserDto) => void,
  signOut: () => void,
}

const AuthContext = createContext<AuthContextValue>(null!);

export const useAuth = () => useContext(AuthContext);

const fetchUser = () => {
  return api.get('my/profile').json<UserDto>();
}

const fetchSignOut = () => {
  return api.post('users/logout');
}

function useAuthProvider() {
  const [user, setUser] = useState<UserDto | null>(null);

  useQuery('user', fetchUser, {
    onSuccess: user => {
      setUser(user);
    }
  });

   const { mutate: signOut } = useMutation(fetchSignOut, {
    onSuccess: () => {
      setUser(null);
    },
  });

  return {
    user,
    setUser,
    signOut,
  };
}

export function AuthProvider({ children }: { children: ReactNode }) {
  const auth = useAuthProvider();
  return (
    <AuthContext.Provider value={auth}>
      {children}
    </AuthContext.Provider>
  );
}