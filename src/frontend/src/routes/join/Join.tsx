import { useReducer } from 'react';

import { UserTypeData, PublicProfileData, LoginCredentialsData } from './components/interfaces';

import AccountTypeStep from './components/AccountTypeStep';
import PublicProfileStep from './components/PublicProfileStep';
import LoginCredentialsStep from './components/LoginCredentialsStep';

const steps = [
  AccountTypeStep,
  PublicProfileStep,
  LoginCredentialsStep
];

type FormData = Partial<UserTypeData & PublicProfileData & LoginCredentialsData>;

interface State {
  step: number,
  data: FormData
}

interface Action {
  type: 'prev' | 'next',
  data: any,
}

function reducer(state: State, action: Action) {
  switch (action.type) {
    case 'prev':
      return {
        step: (state.step > 0) ? state.step - 1 : 0,
        data: {
          ...state.data,
          ...action.data,
        },
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

  const StepComponent = steps[state.step];

  return (
    <StepComponent
      data={state.data}
      currentStep={state.step}
      stepCount={3}
      onNext={data => dispatch({ type: 'next', data })}
      onPrev={data => dispatch({ type: 'prev', data })}
    />
  );
}

export default Join;