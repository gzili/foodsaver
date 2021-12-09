import { Route, Switch } from 'react-router-dom';

import { HubProvider } from 'contexts/hubContext';

import OffersIndex from './offers';

export default function Index() {
  return (
    <HubProvider>
      <Switch>
        <Route path="/app/offers">
          <OffersIndex />
        </Route>
      </Switch>
    </HubProvider>
  );
}