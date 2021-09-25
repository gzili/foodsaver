import Field from '.';
import Input from 'components/form/Input';

const story = {
  title: 'Components/Form/Field',
  decorators: [
    Story => (
      <div style={{ maxWidth: 300 }}>
        <Story />
      </div>
    ),
  ],
  args: {
    label: 'Example Field',
    errorMessage: 'Field is invalid',
    isDisabled: false,
    isInvalid: false,
  },
};

export default story;

const Template = args => (
  <Field id="example-field" {...args}>
    <Input />
  </Field>
);

export const Basic = Template.bind({});

export const Invalid = Template.bind({});
Invalid.args = {
  isInvalid: true,
};

export const WithHelperText = Template.bind({});
WithHelperText.args = {
  helperText: 'This is the helper text',
};