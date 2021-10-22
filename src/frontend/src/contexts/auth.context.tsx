import { createContext, ReactNode, useContext, useState } from 'react';
import { useMutation, useQuery } from 'react-query';

import api from './api.context';

export enum UserType {
  Individual,
  Business,
  Nonprofit
}

export interface User {
  id: number,
  email: string,
  name: string,
  userType: UserType,
}

export interface LoginData {
  email: string,
  password: string,
}

interface AuthContextValue {
  user: User | null,
  setUser: (user: User) => void,
  signOut: () => void,
}

const AuthContext = createContext<AuthContextValue>(null!);

export const useAuth = () => useContext(AuthContext);

const fetchUser = () => {
  return api.get('user').json<User>();
}

const fetchSignOut = () => {
  return api.post('user/logout');
}

function useAuthProvider() {
  const [user, setUser] = useState<User | null>(null);

  useQuery('user', fetchUser, {
    onSuccess: (user: User) => {
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