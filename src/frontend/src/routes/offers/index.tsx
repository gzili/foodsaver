import { Route, Switch } from 'react-router';

import Offers from './Offers';

export default function OffersRoute() {
  return (
    <Switch>
      <Route path="/offers">
        <Offers />
      </Route>
    </Switch>
  );
}