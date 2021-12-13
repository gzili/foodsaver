import { AppBar, BottomBar } from 'components';
import { Route, Switch } from 'react-router';
import Places from './Places';

export default function PlacesIndex() {
  return (
    <Switch>
      <Route path="/app/places/:id"></Route>
      <Route path="/app/places">
        <AppBar />
        <Places />
        <BottomBar />
      </Route>
    </Switch>
  );
}