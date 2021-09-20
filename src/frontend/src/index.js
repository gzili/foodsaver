import React from 'react';
import ReactDOM from 'react-dom';

import { ChakraProvider } from '@chakra-ui/react';

import './index.css';

import App from './containers/App';

ReactDOM.render(
  <React.StrictMode>
      <ChakraProvider>
        <App />
      </ChakraProvider>
  </React.StrictMode>,
  document.getElementById('root')
);
