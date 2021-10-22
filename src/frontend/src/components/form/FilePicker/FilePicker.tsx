import { ChangeEvent, forwardRef, useCallback, useEffect, useMemo, useRef, useState } from 'react';
import { chakra, useToken, Box, Center, Flex, Grid, GridItem, IconButton, Square, Tooltip } from '@chakra-ui/react';
import { useDrop } from 'react-dnd';
import { NativeTypes } from 'react-dnd-html5-backend';

import FaIcon, { faTimes, faFilePdf, faFileWord, faFileExcel, faFile } from 'components/core/FaIcon';

const formatBytes = (bytes: number) => {
  if (bytes < 1024) {
    return bytes + ' bytes';
  } else if (bytes >= 1024 && bytes < 1048576) {
    return +(bytes / 1024).toFixed(1) + ' KB';
  } else if (bytes >= 1048576) {
    return +(bytes / 1048576).toFixed(1) + ' MB';
  } else {
    return '>= 1 GB';
  }
}

interface ImagePreviewProps {
  file: File,
}

const ImagePreview = ({ file }: ImagePreviewProps) => {
  const [imageUrl, setImageUrl] = useState<string | null>(null);

  useEffect(() => {
    const imageUrl = URL.createObjectURL(file);
    setImageUrl(imageUrl);
    return () => URL.revokeObjectURL(imageUrl)
  }, [file]);

  return imageUrl ? (
    <Box pos="absolute" inset={0} bgImage={`url('${imageUrl}')`} bgSize="cover" />
  ): null;
};

interface FileIconProps {
  file: File,
}

const FileIcon = ({ file }: FileIconProps) => {
  const [gray500] = useToken('colors', ['gray.500']);

  if (file.type.startsWith('image')) {
    return <ImagePreview file={file} />;
  }

  const ext = file.name.match(/[.](\w+)$/)?.[1] ?? '';

  if (ext === 'pdf') {
    return <FaIcon icon={faFilePdf} color='#FF1B0E' />;
  } else if (ext.startsWith('doc')) {
    return <FaIcon icon={faFileWord} color='#2B579A' />;
  } else if (ext.startsWith('xls')) {
    return <FaIcon icon={faFileExcel} color='#217346' />;
  } else {
    return <FaIcon icon={faFile} color={gray500} />;
  }
};

interface FileInfoProps {
  file: File,
}

const FileInfo = ({ file }: FileInfoProps) => {
  const {
    name,
    size,
  } = file;

  const hiddenNameRef = useRef<HTMLDivElement>(null!);
  const [isTooltipDisabled, setTooltipDisabled] = useState(true);

  useEffect(() => {
    if (hiddenNameRef.current.scrollWidth > hiddenNameRef.current.offsetWidth) {
      setTooltipDisabled(false);
    }
  }, []);

  return (
    <GridItem pos="relative" minW={0}>
      <Box
        ref={hiddenNameRef}
        pos="absolute"
        w="100%"
        visibility="hidden"
        whiteSpace="nowrap"
        overflowX="scroll"
        fontSize="sm"
        children={name}
      />
      <Tooltip
        isDisabled={isTooltipDisabled}
        gutter={0}
        label={name}
        openDelay={500}
        placement="bottom-start"
        whiteSpace="nowrap"
        fontSize="xs"
        sx={{ maxW: false }}
      >
        <Box
          fontSize="sm"
          whiteSpace="nowrap"
          textOverflow="ellipsis"
          overflow="hidden"
          children={name}
        />
      </Tooltip>
      <Box fontSize="xs" color="gray.500">{formatBytes(size)}</Box>
    </GridItem>
  );
};

interface FileItemProps {
  file: File,
  onRemove: () => void,
}

const FileItem = ({ file, onRemove }: FileItemProps) => {
  return (
    <Grid
      templateColumns="max-content auto max-content"
      gap={2}
      alignItems="center"
      p={2}
      bgColor="gray.100"
      borderRadius="md"
      _notFirst={{ mt: 2 }}
    >
      <GridItem>
        <Square
          pos="relative"
          overflow="hidden"
          size="2rem"
          borderRadius="md"
          bgColor="white"
          fontSize="xl"
        >
          <FileIcon file={file} />
        </Square>
      </GridItem>
      <FileInfo file={file} />
      <GridItem>
        <IconButton
          size="sm"
          color="gray.500"
          borderRadius="md"
          icon={<FaIcon icon={faTimes} />}
          aria-label="Remove file"
          onClick={onRemove}
        />
      </GridItem>
    </Grid>
  );
};

interface AcceptedType {
  kind: 'ext' | 'wildcard' | 'mime',
  matchString: string,
}

const parseAccept = (accept: string | undefined) => {
  if (typeof accept !== 'string') {
    return undefined;
  }

  const acceptedTypes: AcceptedType[] = [];
  const specifiers = accept.split(',');

  for (const s of specifiers) {
    if (s.startsWith('.')) {
      acceptedTypes.push({
        kind: 'ext',
        matchString: s,
      });
    } else if (/^image|audio|video\/\*$/.test(s)) {
      acceptedTypes.push({
        kind: 'wildcard',
        matchString: s.slice(0, -1),
      });
    } else if (s.includes('/')) {
      acceptedTypes.push({
        kind: 'mime',
        matchString: s,
      });
    }
  }

  return acceptedTypes;
}

const isAcceptedType = (file: File, acceptedTypes: ReturnType<typeof parseAccept>) => {
  if (!Array.isArray(acceptedTypes)) {
    return true;
  }

  for (const { kind, matchString } of acceptedTypes) {
    if ((kind === 'ext') && file.name.endsWith(matchString)) return true;
    else if ((kind === 'mime') && (file.type === matchString)) return true;
    else if ((kind === 'wildcard') && file.type.startsWith(matchString)) return true;
  }

  return false;
}

interface RejectionInfo {
  reason: 'noMultiple' | 'wrongType',
  items: File[],
}

interface FilePickerProps {
  id: string,
  value: File[],
  onChange: (files: File[]) => void,
  onReject?: (rejection: RejectionInfo) => void,
  accept?: string,
  multiple?: boolean,
  isDisabled?: boolean,
  isInvalid?: boolean,
}

const FilePicker = forwardRef<HTMLInputElement, FilePickerProps>((props, ref) => {
  const {
    id,
    value: files,
    onChange: setFiles,
    onReject,
    accept,
    multiple,
    isInvalid,
    isDisabled,
  } = props;

  const acceptedTypes = useMemo(() => parseAccept(accept), [accept]);

  const handleReject = useCallback((reason: RejectionInfo['reason'], items: RejectionInfo['items']) => {
    onReject && onReject({
      reason,
      items,
    });
  }, [onReject])

  const handleChange = useCallback((items: File[]) => {
    // TODO: reject if directory

    if (!multiple && (items.length > 1)) {
      handleReject('noMultiple', items);
    } else {
      const acceptedFiles: File[] = [];
      const rejectedFiles: File[] = [];

      for (const file of items) {
        isAcceptedType(file, acceptedTypes) ? acceptedFiles.push(file) : rejectedFiles.push(file);
      }

      if (onReject && (rejectedFiles.length > 0)) {
        handleReject('wrongType', rejectedFiles);
      }

      if (acceptedFiles.length > 0) {
        if (multiple) {
          // TODO: confirm replace if a file with the same name already exists
          const noDuplicates = files.filter(existingFile => {
            for (const newFile of acceptedFiles) {
              if (newFile.name === existingFile.name) {
                return false;
              }
            }
            return true;
          });
          setFiles([...noDuplicates, ...acceptedFiles]);
        } else {
          setFiles(acceptedFiles);
        }
      }
    }
  }, [multiple, acceptedTypes, handleReject, onReject, files, setFiles]);

  const handleNativeInputChange = useCallback((e: ChangeEvent<HTMLInputElement>) => {
    handleChange([...e.target.files!]); // convert FileList to array by spreading
  }, [handleChange]);

  const handleDrop = useCallback((item: DataTransfer) => {
    handleChange([...item.files]); // convert FileList yet again
  }, [handleChange]);

  const [{ canDrop, isOver }, drop] = useDrop(() => ({
    accept: [NativeTypes.FILE],
    drop: (item: DataTransfer) => handleDrop(item),
    collect: monitor => ({
      canDrop: monitor.canDrop(),
      isOver: monitor.isOver(),
    }),
  }), [handleDrop]);

  const handleRemove = (file: File) => {
    setFiles(files.filter(f => f !== file));
  };

  // TODO: sr-only input and highlight label when input is in focused
  return (
    <Box
      pos="relative"
      bg={(canDrop && isOver) ? 'brand.50' : undefined}
      borderWidth="2px"
      borderStyle="solid"
      borderColor={canDrop ? (isOver ? 'brand.500' : 'gray.400') : 'gray.300'}
      borderRadius="md"
      transitionProperty="common"
      transitionDuration="normal"
      _hover={{
        borderColor: 'gray.400',
      }}
      _invalid={{
        borderColor: 'red.500',
      }}
      _disabled={{
        opacity: 0.5,
        pointerEvents: 'none',
      }}
      aria-invalid={isInvalid}
      aria-disabled={isDisabled}
      ref={drop}
    >
      {((files.length === 0) || multiple) && (
        <Center h="4rem">
          <chakra.input
            pos="absolute"
            opacity={0}
            pointerEvents="none"
            ref={ref}
            id={id}
            type="file"
            accept={accept}
            multiple={multiple}
            onChange={handleNativeInputChange}
          />
          <Flex>
            <Box pointerEvents="none">Drag file{multiple && 's'} here</Box>
            {!canDrop && (
              <>
                <Box>&nbsp;or&nbsp;</Box>
                <Box
                  as="label"
                  htmlFor={id}
                  color="brand.500"
                  fontWeight="bold"
                  cursor="pointer"
                  _hover={{
                    color: 'brand.600',
                  }}
                  _focus={{
                    bgColor: 'rgba(213, 119, 67, 0.2)',
                    outline: 'none',
                  }}
                >
                  browse
                </Box>
              </>
            )}
          </Flex>
        </Center>
      )}
      {(files.length > 0) && (
        <Box pt={!multiple ? 2 : undefined} pb={2} px={2}>
          {files.map(file => (
            <FileItem key={file.name} file={file} onRemove={() => handleRemove(file)} />
          ))}
        </Box>
      )}
    </Box>
  );
});

export default FilePicker;

/*
// Checks if the file is a directory
for (const file of item.files) {
  let isFile = true;
  await file.slice(0, 1).arrayBuffer().catch(() => false);
  console.log(isFile);
}
*/