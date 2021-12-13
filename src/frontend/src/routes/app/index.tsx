import { Route, Switch } from 'react-router-dom';

import { HubProvider } from 'contexts/hubContext';

import MyIndex from './my';
import OffersIndex from './offers';
import PlacesIndex from './places';

export default function Index() {
  return (
    <HubProvider>
      <Switch>
        <Route path="/app/my">
          <MyIndex />
        </Route>
        <Route path="/app/places">
          <PlacesIndex />
        </Route>
        <Route path="/app/offers">
          <OffersIndex />
        </Route>
      </Switch>
    </HubProvider>
  );
}