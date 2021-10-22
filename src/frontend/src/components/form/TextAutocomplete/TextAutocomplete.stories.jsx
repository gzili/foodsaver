import TextAutocomplete from '.';

import { useState } from 'react';

import { distilleries } from 'services/dummyData';

const story = {
  title: 'Components/Form/TextAutocomplete',
  decorators: [
    Story => (
      <div style={{ maxWidth: 300/*, height: 2000, display: 'flex', alignItems: 'center', overflowY: 'auto'*/ }}>
        <Story />
      </div>
    ),
  ],
  args: {
    isDisabled: false,
    isInvalid: false,
  },
};

export default story;

const Template = args => {
  const [value, setValue] = useState('');
  return <TextAutocomplete items={distilleries} value={value} onChange={setValue} {...args} />;
}

const withArgs = args => {
  const t = Template.bind({});
  t.args = args;
  return t;
};

export const Basic = Template.bind({});

export const Disabled = withArgs({ isDisabled: true });

export const Invalid = withArgs({ isInvalid: true });