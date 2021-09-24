import { Route, Switch } from 'react-router-dom';

import Home from 'routes';
import Login from 'routes/login';
import Join from 'routes/join';

function App() {
  return (
    <Switch>
      <Route path="/join">
        <Join />
      </Route>
      <Route path="/login">
        <Login />
      </Route>
      <Route path="/">
        <Home />
      </Route>
    </Switch>
  );
}

export default App;
