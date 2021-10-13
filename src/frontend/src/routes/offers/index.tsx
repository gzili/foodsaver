import { Route, Switch } from 'react-router';

import Offer from './[id]';
import Offers from './Offers';

export default function OffersRoute() {
  return (
    <Switch>
      <Route path="/offers/:id">
        <Offer />
      </Route>
      <Route path="/offers">
        <Offers />
      </Route>
    </Switch>
  );
}