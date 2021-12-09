import { Route, Switch } from 'react-router';

import { AppBar } from 'components';

import Offer from './[id]';
import Offers from './Offers';

export default function Index() {
  return (
    <Switch>
      <Route path="/app/offers/:id">
        <Offer />
      </Route>
      <Route path="/app/offers">
        <AppBar />
        <Offers />
      </Route>
    </Switch>
  );
}