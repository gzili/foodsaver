import { AppBar, BottomBar } from 'components';
import { Route, Switch } from 'react-router';
import Place from './[id]';
import Places from './Places';

export default function PlacesIndex() {
  return (
    <Switch>
      <Route path="/app/places/:id">
        <AppBar />
        <Place />
        <BottomBar />
      </Route>
      <Route path="/app/places">
        <AppBar />
        <Places />
        <BottomBar />
      </Route>
    </Switch>
  );
}