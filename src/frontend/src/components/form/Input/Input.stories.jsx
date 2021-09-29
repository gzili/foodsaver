import Input from 'components/form/Input';

const story = {
  title: 'Components/Form/Input',
  component: Input,
  decorators: [
    Story => (
      <div style={{ maxWidth: 300 }}>
        <Story />
      </div>
    ),
  ],
  args: {
    isDisabled: false,
    isInvalid: false,
    placeholder: '',
    leftAddon: '',
    rightAddon: '',
  },
};

export default story;

const Template = args => <Input {...args} />;

export const Basic = Template.bind({});

export const Placeholder = Template.bind({});
Placeholder.args = { placeholder: 'Placeholder' };

export const Disabled = Template.bind({});
Disabled.args = { isDisabled: true };

export const Invalid = Template.bind({});
Invalid.args = { isInvalid: true };

export const WithLeftAddon = Template.bind({});
WithLeftAddon.args = { leftAddon: '£' };

export const WithRightAddon = Template.bind({});
WithRightAddon.args = { rightAddon: '%' };