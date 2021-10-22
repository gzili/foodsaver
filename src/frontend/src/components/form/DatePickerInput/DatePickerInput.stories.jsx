import { useState } from 'react';
import DatePickerInput from '.';

const story = {
  title: 'Components/Form/DatePickerInput',
  args: {
    isDisabled: false,
    isInvalid: false,
    disableFuture: false,
    disablePast: false,
    weekStartsOnMonday: false,
    disableCloseOnPick: false,
    viewOnOpen: 'd',
    smallestUnit: 'd',
  },
  argTypes: {
    viewOnOpen: {
      options: ['d', 'm', 'y'],
      control: { type: 'select' },
    },
    smallestUnit: {
      options: ['d', 'm', 'y'],
      control: { type: 'select' },
    },
  },
  decorators: [
    Story => (
      <div style={{ maxWidth: 300 }}>
        <Story />
      </div>
    ),
  ],
};

export default story;

const Template = args => {
  const [date, setDate] = useState(null);
  return <DatePickerInput value={date} onChange={setDate} {...args} />
};

export const Default = Template.bind({});