import { Route, Switch } from 'react-router-dom';

import { BottomBar } from 'components';

import My from './My';

export default function MyIndex() {
  return (
    <Switch>
      <Route path="/app/my">
        <My />
        <BottomBar />
      </Route>
    </Switch>
  );
}