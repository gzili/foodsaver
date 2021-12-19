import { Route, Switch } from 'react-router-dom';

import { AppBar, BottomBar } from 'components';

import My from './My';
import Offers from './offers';
import Reservations from './reservations';

export default function MyIndex() {
  return (
    <Switch>
      <Route path="/app/my/offers">
        <AppBar />
        <Offers />
        <BottomBar />
      </Route>
      <Route path="/app/my/reservations">
        <AppBar />
        <Reservations />
        <BottomBar />
      </Route>
      <Route path="/app/my">
        <My />
        <BottomBar />
      </Route>
    </Switch>
  );
}