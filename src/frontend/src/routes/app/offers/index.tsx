import { Route, Switch } from 'react-router';

import { AppBar, BottomBar } from 'components';
import { useAuth } from 'contexts/authContext';

import Offer from './[id]';
import Offers from './Offers';

export default function Index() {
  const { user } = useAuth();

  return (
    <Switch>
      <Route path="/app/offers/:id">
        <Offer />
      </Route>
      <Route path="/app/offers">
        <AppBar />
        <Offers />
        {user && <BottomBar />}
      </Route>
    </Switch>
  );
}