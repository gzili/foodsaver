import './App.css';
import {Route, Switch} from "react-router-dom";
import Home from "routes";

function App() {
  return (
    <Switch>
      <Route path="/">
        <Home />
      </Route>
    </Switch>
  );
}

export default App;
