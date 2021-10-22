import FilePicker from '.';

import { useCallback, useState } from 'react';
import { DndProvider } from 'react-dnd';
import { HTML5Backend } from 'react-dnd-html5-backend';

const Wrapper = props => {
  const [files, setFiles] = useState([]);

  const onChange = useCallback(v => setFiles(v), [setFiles]);
  const onReject = useCallback(r => console.log(r), []);

  return <FilePicker name='file-picker' value={files} onChange={onChange} onReject={onReject} {...props} />;
}

const story = {
  title: 'Components/Form/FilePicker',
  decorators: [
    Story => (
      <DndProvider backend={HTML5Backend}>
        <div style={{ maxWidth: 300 }}>
          <Story />
        </div>
      </DndProvider>
    ),
  ],
  args: {
    fullWidth: false,
    isDisabled: false,
    isInvalid: false,
    multiple: false,
  },
  argTypes: {
    multiple: {
      table: {
        disable: true,
      },
    },
  },
};

export default story;

const Template = args => <Wrapper {...args} />

export const Default = Template.bind({});

export const Multiple = Template.bind({});

Multiple.args = {
  multiple: true,
};