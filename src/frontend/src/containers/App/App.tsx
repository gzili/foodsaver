import { Route, Switch } from 'react-router-dom';

import { AppBar } from 'components/layout';
import AppIndex from 'routes/app';
import Home from 'routes';
import Join from 'routes/join';
import Login from 'routes/login';

function App() {
  return (
    <Switch>
      <Route path="/app">
        <AppIndex />
      </Route>
      <Route path="/join">
        <Join />
      </Route>
      <Route path="/login">
        <Login />
      </Route>
      <Route path="/">
        <AppBar />
        <Home />
      </Route>
    </Switch>
  );
}

export default App;
