import React from 'react';
import ReactDOM from 'react-dom';

import { ChakraProvider } from '@chakra-ui/react';

import './index.css';

import App from './containers/App';
import {BrowserRouter} from "react-router-dom";

ReactDOM.render(
  <React.StrictMode>
      <ChakraProvider>
          <BrowserRouter>
            <App />
          </BrowserRouter>
      </ChakraProvider>
  </React.StrictMode>,
  document.getElementById('root')
);
