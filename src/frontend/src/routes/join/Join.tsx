import AccountTypeFlow from './components/AccountTypeFlow';

const flows = [
  AccountTypeFlow
];

function Join() {
  const FlowComponent = flows[0];

  const handleNext = (data: any) => {
    console.log(data);
  };

  return (
    <FlowComponent onNext={data => handleNext(data)} />
  );
}

export default Join;