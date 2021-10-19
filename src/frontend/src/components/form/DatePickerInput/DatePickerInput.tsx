import { forwardRef, useCallback, useEffect, useMemo, useRef, useState, ReactNode } from 'react';
import {
  addDays,
  addMonths,
  compareAsc,
  endOfDay,
  endOfMonth,
  format,
  getDate,
  getYear,
  isFuture,
  isPast,
  isSameMonth,
  isThisMonth,
  isThisYear,
  isToday,
  isValid,
  lightFormat,
  parse,
  setMonth,
  setYear,
  startOfDay,
  startOfMonth,
  startOfWeek,
  startOfYear,
  subMonths,
  toDate
} from 'date-fns';
import {
  useDisclosure,
  Box,
  Button,
  Center,
  Flex,
  IconButton,
  Popover,
  PopoverTrigger,
  PopoverContent,
  FlexProps,
  IconButtonProps,
  BoxProps,
  ButtonProps
} from '@chakra-ui/react';
import FaIcon, { faCalendar, faAngleLeft, faAngleRight } from 'components/core/FaIcon';

import RifmInput from '../RifmInput';
import { InputProps } from '../Input';
import { dateFormatProps, monthFormatProps, yearFormatProps } from './formatProps';

type DateUnit = 'd' | 'm' | 'y';

interface DatePickerProps {
  value: Date | null,
  onChange: (value: Date | null) => void,
  disableCloseOnPick?: boolean,
  disableFuture?: boolean,
  disablePast?: boolean,
  isDisabled?: boolean,
  smallestUnit?: DateUnit,
  viewOnOpen?: DateUnit,
  weekStartsOnMonday?: boolean,
}

interface CalendarTriggerProps extends Required<Omit<DatePickerProps, 'value' | 'onChange'>> {
  selectedDate: Date | null,
  onChange: (date: Date) => void,
}

type CalendarWidgetProps = Omit<CalendarTriggerProps, 'disableCloseOnPick' | 'isDisabled'>;

interface ViewProps {
  commitDate: (date: Date) => void,
  disableFuture: boolean,
  disablePast: boolean,
  internalDate: Date,
  selectedDate: Date | null,
  setInternalDate: (date: Date) => void,
  setView: (view: DateUnit) => void,
  smallestUnit: DateUnit,
  weekStartsOnMonday: boolean,
}

type DayViewProps = Omit<ViewProps, 'smallestUnit'>;

type MonthViewProps = Omit<ViewProps, 'selectedDate' | 'weekStartsOnMonday'>;

type YearViewProps = Omit<ViewProps, 'weekStartsOnMonday'>;

interface PickerButtonProps extends ButtonProps {
  isCurrent?: boolean,
  isLightened?: boolean,
  isSelected?: boolean,
}

const WEEKDAYS  = [ 'Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat' ];
const SHORT_MONTHS = [ 'Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec' ];
const CELL_SIZE = 10;

const PrevNextButton = (props: IconButtonProps) => (
  <IconButton
    variant="ghost"
    h={8}
    minW={8}
    color="gray.500"
    {...props}
  />
);

const ViewButton = (props: ButtonProps) => (
  <Button
    h={7}
    {...props}
  />
);

const CalendarBody = (props: FlexProps) => (
  <Flex
    direction="column"
    justify="center"
    align="center"
    h={`${(6 * CELL_SIZE + 6) * 4}px`}
    w={`${7 * CELL_SIZE * 4}px`}
    {...props}
  />
);

const CalendarHeader = (props: FlexProps) => (
  <Flex align="center" justify="center" h={8} mb={2} {...props} />
);

const Calendar = (props: BoxProps) => (
  <Box
    p={4}
    bg="white"
    boxShadow="popover"
    borderRadius="md"
    {...props}
  />
);

const PickerButton = (props: PickerButtonProps) => {
  const {
    isCurrent,
    isLightened,
    isSelected,
    ...buttonProps
  } = props;

  let color;
  if (isSelected) {
    color = 'white';
  } else if (isCurrent) {
    color = 'brand.500';
  } else if (isLightened) {
    color = 'gray.400';
  } else {
    color = 'gray.600';
  }

  return (
    <Button
      h="100%"
      minW="100%"
      p={0}
      bg={isSelected ? 'brand.500' : 'none'}
      color={color}
      fontWeight={isCurrent ? 'bold' : 'normal'}
      _hover={{
        bg: isSelected ? 'brand.600' : 'gray.100',
      }}
      _active={{
        bg: isSelected ? 'brand.700' : 'gray.200',
      }}
      {...buttonProps}
    />
  );
};

const YearView = (props: YearViewProps) => {
  const {
    commitDate,
    disableFuture,
    disablePast,
    internalDate,
    selectedDate,
    setInternalDate,
    setView,
    smallestUnit,
  } = props;

  const containerRef = useRef<HTMLDivElement>(null);
  const targetRef = useRef<HTMLDivElement>(null);

  const handleYearChange = (year: number) => {
    if (smallestUnit === 'y') {
      commitDate(startOfYear(setYear(internalDate, year)));
    } else {
      setInternalDate(setYear(internalDate, year));
      setView('m');
    }
  };

  const selectedYear = getYear(selectedDate || NaN);
  const internalYear = getYear(internalDate);
  const thisYear = getYear(new Date());

  const minYear = disablePast ? thisYear : 1900;
  const maxYear = disableFuture ? thisYear : 2100;

  const yearCells = [];
  for (let y = minYear; y <= maxYear; ++y) {
    yearCells.push(
      <Box
        key={y}
        ref={selectedYear ? (y === selectedYear ? targetRef : undefined) : (y === internalYear ? targetRef : undefined)}
        h={CELL_SIZE}
        w="25%"
        p={1}
      >
        <PickerButton
          isCurrent={y === thisYear}
          //isSelected={y === selectedYear}
          onClick={() => handleYearChange(y)}
          children={y}
        />
      </Box>
    );
  }

  useEffect(() => {
    const container = containerRef.current;
    const target = targetRef.current;

    if (target && container) {
      container.scrollTo(0, target.offsetTop - Math.floor(container.offsetHeight / 2) - Math.floor(target.offsetHeight) + 4);
    }
  }, []);

  return (
    <Calendar>
      <CalendarHeader fontWeight="bold">{`${minYear} - ${maxYear}`}</CalendarHeader>
      <CalendarBody>
        <Flex wrap="wrap" h="100%" w="100%" overflowY="auto" ref={containerRef}>
          {yearCells}
        </Flex>
      </CalendarBody>
    </Calendar>
  );
};

const MonthView = (props: MonthViewProps) => {
  const {
    commitDate,
    disableFuture,
    disablePast,
    internalDate,
    setInternalDate,
    setView,
    smallestUnit
  } = props;

  const handleMonthChange = (month: number) => {
    if (smallestUnit === 'm') {
      commitDate(startOfMonth(setMonth(internalDate, month)))
    } else {
      setInternalDate(setMonth(internalDate, month));
      setView('d');
    }
  };

  const rowNodes = new Array(4);
  for (let rowIndex = 0; rowIndex < 4; ++rowIndex) {
    const cellNodes = new Array(3);
    for (let cellIndex = 0; cellIndex < 3; ++cellIndex) {
      const monthIndex = rowIndex * 3 + cellIndex;
      const date = setMonth(internalDate, monthIndex);
      cellNodes[cellIndex] = (
        <Box key={cellIndex} h={12} w={20} p={1}>
          <PickerButton
            isCurrent={isThisYear(date) && isThisMonth(date)}
            isDisabled={
              (disablePast && isPast(endOfMonth(date))) ||
              (disableFuture && isFuture(startOfMonth(date)))
            }
            onClick={() => handleMonthChange(monthIndex)}
          >
            {SHORT_MONTHS[monthIndex]}
          </PickerButton>
        </Box>
      );
    }
    rowNodes[rowIndex] = <Flex key={rowIndex}>{cellNodes}</Flex>;
  }

  return (
    <Calendar>
      <CalendarHeader>
        <ViewButton onClick={() => setView('y')}>{lightFormat(internalDate, 'yyyy')}</ViewButton>
      </CalendarHeader>
      <CalendarBody>
        <Box>
          {rowNodes}
        </Box>
      </CalendarBody>
    </Calendar>
  );
};

const Weekday = ({ children }: { children: ReactNode }) => {
  return (
    <Center w={CELL_SIZE} h={6} fontSize="xs" fontWeight="bold" color="gray.500">{children}</Center>
  );
};

const DayView = (props: DayViewProps) => {
  const {
    commitDate,
    disableFuture,
    disablePast,
    internalDate,
    selectedDate,
    setInternalDate,
    setView,
    weekStartsOnMonday,
  } = props;

  const normalizedSelectedDate = startOfDay(selectedDate || NaN);
  const prevMonth = subMonths(internalDate, 1);
  const nextMonth = addMonths(internalDate, 1);
  const disablePrev = disablePast && isPast(endOfMonth(prevMonth));
  const disableNext = disableFuture && isFuture(startOfMonth(nextMonth));

  const weekdayNodes = new Array(7);
  for (let i = 0; i < 7; ++i) {
    const weekday = WEEKDAYS[weekStartsOnMonday ? (i + 1) % 7 : i];
    weekdayNodes[i] = <Weekday key={weekday}>{weekday}</Weekday>
  }
  
  let currentDate = startOfWeek(startOfMonth(internalDate), { weekStartsOn: weekStartsOnMonday ? 1 : 0 });
  const rowNodes = new Array(6);
  for (let rowIndex = 0; rowIndex < 6; ++rowIndex) {
    const cellNodes = new Array(7);
    for (let cellIndex = 0; cellIndex < 7; ++cellIndex) {
      const cellDate = toDate(currentDate);
      cellNodes[cellIndex] = (
        <Box key={cellIndex} w={CELL_SIZE} h={CELL_SIZE} p={1}>
          <PickerButton
            isCurrent={isToday(cellDate)}
            isDisabled={(disableFuture && isFuture(cellDate)) || (disablePast && isPast(endOfDay(cellDate)))}
            isLightened={!isSameMonth(cellDate, internalDate)}
            isSelected={compareAsc(normalizedSelectedDate, cellDate) === 0}
            onClick={() => commitDate(cellDate)}
            children={getDate(cellDate)}
          />
        </Box>
      );
      currentDate = addDays(currentDate, 1);
    }
    rowNodes[rowIndex] = (
      <Flex key={rowIndex}>{cellNodes}</Flex>
    );
  }

  return (
    <Calendar>
      <CalendarHeader justify="space-between">
        <PrevNextButton
          aria-label="Previous month"
          icon={<FaIcon icon={faAngleLeft} />}
          onClick={() => setInternalDate(prevMonth)}
          isDisabled={disablePrev}
        />
        <ViewButton onClick={() => setView('m')}>{format(internalDate, 'MMMM yyyy')}</ViewButton>
        <PrevNextButton
          aria-label="Next month"
          icon={<FaIcon icon={faAngleRight} />}
          onClick={() => setInternalDate(nextMonth)}
          isDisabled={disableNext}
        />
      </CalendarHeader>
      <CalendarBody>
        <Flex>
          {weekdayNodes}
        </Flex>
        <Box>
          {rowNodes}
        </Box>
      </CalendarBody>
    </Calendar>
  );
};

const getViewsBySmallestUnit = (smallestUnit: DateUnit): DateUnit[] => {
  switch (smallestUnit) {
    case 'y':
      return ['y'];
    case 'm':
      return ['m', 'y'];
    default:
      return ['d', 'm', 'y'];
  }
};

const CalendarWidget = (props: CalendarWidgetProps) => {
  const {
    disableFuture,
    disablePast,
    onChange,
    selectedDate,
    smallestUnit,
    viewOnOpen,
    weekStartsOnMonday,
  } = props;

  const views = useMemo(() => getViewsBySmallestUnit(smallestUnit), [smallestUnit]);

  const [internalDate, setInternalDate] = useState(isValid(selectedDate) ? selectedDate as Date : new Date());
  const [view, setView] = useState(views.includes(viewOnOpen) ? viewOnOpen : views[0]);

  switch (view) {
    case 'y':
      return (
        <YearView
          commitDate={onChange}
          disableFuture={disableFuture}
          disablePast={disablePast}
          internalDate={internalDate}
          selectedDate={selectedDate}
          setInternalDate={setInternalDate}
          setView={setView}
          smallestUnit={smallestUnit}
        />
      );
    case 'm':
      return (
        <MonthView
          commitDate={onChange}
          disableFuture={disableFuture}
          disablePast={disablePast}
          internalDate={internalDate}
          setInternalDate={setInternalDate}
          setView={setView}
          smallestUnit={smallestUnit}
        />
      );
    default:
      return (
        <DayView
          commitDate={onChange}
          disableFuture={disableFuture}
          disablePast={disablePast}
          internalDate={internalDate}
          selectedDate={selectedDate}
          setInternalDate={setInternalDate}
          setView={setView}
          weekStartsOnMonday={weekStartsOnMonday}
        />
      );
  }
};

const TriggerWithPopover = (props: CalendarTriggerProps) => {
  const { disableCloseOnPick, isDisabled, onChange, ...widgetProps } = props;

  const { isOpen, onToggle, onClose } = useDisclosure();

  const handleChange = (date: Date) => {
    onChange(date);
    !disableCloseOnPick && onClose();
  };

  return (
    <Popover isOpen={isOpen} onClose={onClose} isLazy gutter={12}>
      <PopoverTrigger>
        <IconButton
          aria-label="Trigger calendar"
          h={7}
          minW={7}
          color="gray.500"
          icon={<FaIcon icon={faCalendar} />}
          isDisabled={isDisabled}
          onClick={onToggle}
        />
      </PopoverTrigger>
      <PopoverContent
        sx={{
          w: false,
          bg: false,
          border: false,
          borderRadius: false,
          boxShadow: false,
        }}
      >
          <CalendarWidget onChange={handleChange} {...widgetProps} />
      </PopoverContent>
    </Popover>
  );
};

const getFormatBySmallestUnit = (unit: DateUnit) => {
  let rifmArgs;
  let formatString;
  let formatRegex;
  let placeholder;

  switch (unit) {
    case 'y':
      rifmArgs = yearFormatProps;
      formatString = 'yyyy';
      formatRegex = /\d{4}/;
      placeholder = 'YYYY';
      break;
    case 'm':
      rifmArgs = monthFormatProps;
      formatString = 'MM/yyyy';
      formatRegex = /\d{2}\/\d{4}/;
      placeholder = 'MM/YYYY';
      break;
    default:
      rifmArgs = dateFormatProps;
      formatString = 'dd/MM/yyyy';
      formatRegex = /\d{2}\/\d{2}\/\d{4}/;
      placeholder = 'DD/MM/YYYY';
  }

  const { shouldMask, ...restRifmArgs } = rifmArgs;

  return {
    rifmArgs: restRifmArgs,
    shouldMask,
    formatString,
    formatRegex,
    placeholder
  };
};

const DatePickerInput = forwardRef<HTMLInputElement, Omit<InputProps, 'value' | 'onChange'> & DatePickerProps>((props, ref) => {
  const {
    onChange,
    value,
    disableCloseOnPick = false,
    disableFuture = false,
    disablePast = false,
    isDisabled = false,
    smallestUnit = 'd',
    viewOnOpen = 'd',
    weekStartsOnMonday = false,
    ...InputProps
  } = props;

  const [inputValue, setInputValue] = useState('');

  const {
    rifmArgs,
    shouldMask,
    formatString,
    formatRegex,
    placeholder
  } = useMemo(() => getFormatBySmallestUnit(smallestUnit), [smallestUnit]);

  const parseInputValue = useCallback((value: string) => {
    if (value === '' || formatRegex.test(value) === false) return null;
    return parse(value, formatString, new Date());
  }, [formatString, formatRegex]);

  const handleInputChange = (value: string) => {
    setInputValue(value);
    onChange(parseInputValue(value));
  };

  useEffect(() => {
    if (isValid(value)) {
      setInputValue(lightFormat(value || NaN, formatString))
    }
  }, [value, formatString]);

  return (
    <RifmInput
      rifmProps={{
        ...rifmArgs,
        mask: shouldMask(inputValue),
      }}
      ref={ref}
      value={inputValue}
      onChange={handleInputChange}
      placeholder={placeholder}
      {...InputProps}
      rightElement={
        <TriggerWithPopover
          disableCloseOnPick={disableCloseOnPick}
          disableFuture={disableFuture}
          disablePast={disablePast}
          isDisabled={isDisabled}
          onChange={onChange}
          selectedDate={value}
          smallestUnit={smallestUnit}
          viewOnOpen={viewOnOpen}
          weekStartsOnMonday={weekStartsOnMonday}
        />
      }
    />
  );
});

export default DatePickerInput;