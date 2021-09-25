import { useReducer } from 'react';

import { AccountTypeData, PublicProfileData, LoginCredentialsData } from './components/interfaces';

import AccountTypeStep from './components/AccountTypeStep';
import PublicProfileStep from './components/PublicProfileStep';
import LoginCredentialsStep from './components/LoginCredentialsStep';

const steps = [
  AccountTypeStep,
  PublicProfileStep,
  LoginCredentialsStep
];

type FormData = Partial<AccountTypeData & PublicProfileData & LoginCredentialsData>;

interface State {
  step: number,
  data: FormData
}

interface PrevAction {
  type: 'prev'
}

interface NextAction {
  type: 'next',
  data: AccountTypeData | PublicProfileData | LoginCredentialsData,
}

type Action = PrevAction | NextAction;

function reducer(state: State, action: Action) {
  switch (action.type) {
    case 'prev':
      return {
        step: (state.step > 0) ? state.step - 1 : 0,
        data: state.data,
      };
    case 'next':
      return {
        step: state.step + 1,
        data: {
          ...state.data,
          ...action.data,
        }
      };
    default:
      throw new Error('Invalid state action');
  }
}

function Join() {
  const [state, dispatch] = useReducer(reducer, {
    step: 0,
    data: {}
  });
  
  console.log(state.data);

  const FlowComponent = steps[state.step];

  return (
    <FlowComponent
      data={state.data}
      currentStep={state.step}
      stepCount={3}
      onNext={data => dispatch({ type: 'next', data })}
      onPrev={() => dispatch({ type: 'prev' })}
    />
  );
}

export default Join;