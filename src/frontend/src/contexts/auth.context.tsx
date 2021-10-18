import { createContext, ReactNode, useContext, useState } from 'react';
import { useQuery } from 'react-query';

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
}

const AuthContext = createContext<AuthContextValue>(null!);

export const useAuth = () => useContext(AuthContext);

// const fetchLogin = async (data: LoginData) => {
//   return api.post('users/login', { json: data }).json<User>();
// }

const fetchUser = () => {
  return api.get('users').json<User>();
}

function useAuthProvider() {
  const [user, setUser] = useState<User | null>(null);

  useQuery('user', fetchUser, {
    onSuccess: (user: User) => {
      setUser(user);
    }
  });

  return {
    user,
    setUser,
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