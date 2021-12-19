import { Route, Switch } from 'react-router-dom';

import { AppBar, BottomBar } from 'components';

import Home from './Home';

export default function HomeIndex() {
  return (
    <Switch>
      <Route path="/app/home">
        <AppBar />
        <Home />
        <BottomBar />
      </Route>
    </Switch>
  );
}