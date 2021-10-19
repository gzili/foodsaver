import { useCallback, useState } from 'react';

export function useAsyncOrLocalData<T>(asyncFnOrValue: T | (() => Promise<T>), immediate = false) {
  const [isLoading, setIsLoading] = useState(false);
  const [isError, setIsError] = useState(false);
  const [data, setData] = useState<T | null>(null);
  const [error, setError] = useState<Error | null>(null);

  const getData = useCallback(() => {
    if (asyncFnOrValue instanceof Function) {
      setIsLoading(true);

      asyncFnOrValue()
      .then((value: T) => {
        setData(value)
        setError(null);
        setIsError(false);
      })
      .catch((error: Error) => {
        setError(error);
        setData(null);
        setIsError(true);
      })
      .then(() => {
        setIsLoading(false);
      });
    } else {
      setData(asyncFnOrValue);
    }
  }, [asyncFnOrValue]);

  if (immediate) getData();

  return {
    getData,
    isLoading,
    isError,
    data,
    error,
  };
}