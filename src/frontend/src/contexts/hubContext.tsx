import * as signalR from '@microsoft/signalr';
import type { HubConnection } from '@microsoft/signalr';
import { createContext, useContext, useEffect, useMemo, useState } from 'react';
import type { ReactNode } from 'react';

interface HubContextValue {
  connection: HubConnection,
  isConnected: boolean,
}

const hubConnection = new signalR.HubConnectionBuilder().withUrl('/hub').build();

const hubContext = createContext<HubContextValue>({
  connection: hubConnection,
  isConnected: false,
});

export const useHub = () => useContext(hubContext);

interface HubProviderProps {
  children: ReactNode
}

export function HubProvider({ children }: HubProviderProps) {
  const [isConnected, setIsConnected] = useState(false);

  useEffect(() => {
    hubConnection.start()
      .then(() => {
        setIsConnected(true);
      })
      .catch(err => {
        console.log(err);
      });
  }, []);

  const value = useMemo(() => ({
    connection: hubConnection,
    isConnected,
  }), [isConnected]);

  return (
    <hubContext.Provider value={value}>
      {children}
    </hubContext.Provider>
  );
}