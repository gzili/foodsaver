import React from 'react';
import ReactDOM from 'react-dom';

import { ChakraProvider } from '@chakra-ui/react';
import { BrowserRouter } from "react-router-dom";

import theme from './theme';

import App from './containers/App';

import './index.css';

ReactDOM.render(
  <React.StrictMode>
      <ChakraProvider theme={theme}>
          <BrowserRouter>
            <App />
          </BrowserRouter>
      </ChakraProvider>
  </React.StrictMode>,
  document.getElementById('root')
);
