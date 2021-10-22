const parseDigits = (str: string) => (str.match(/\d+/g) || []).join('');

const formatDate = (str: string) => {
  const digits = parseDigits(str);
  const chars = digits.split('');
  return chars
    .reduce(
      (r, v, index) => (index === 2 || index === 4 ? `${r}/${v}` : `${r}${v}`), // 4, 6 for YYYY-MM-DD
      ''
    )
    .substr(0, 10);
};

const formatMonth = (str: string) => {
  const digits = parseDigits(str);
  const chars = digits.split('');
  return chars
    .reduce(
      (r, v, index) => (index === 2 ? `${r}/${v}` : `${r}${v}`),
      ''
    )
    .substr(0, 7);
};

const formatYear = (str: string) => {
  const digits = parseDigits(str);
  return digits.substr(0, 4);
};

const formatDateWithAppend = (str: string) => {
  const res = formatDate(str);

  if (str.endsWith('/')) {
    if (res.length === 2) { // 4 for YYYY-MM-DD
      return `${res}/`;
    }

    if (res.length === 5) { // 7 for YYYY-MM-DD
      return `${res}/`;
    }
  }
  return res;
};

const formatMonthWithAppend = (str: string) => {
  const res = formatMonth(str);

  if (str.endsWith('/')) {
    if (res.length === 2) {
      return `${res}/`;
    }
  }
  return res;
};

const appendDateSeparator = (v: string) => (v.length === 2 || v.length === 5 ? `${v}/` : v); // 4, 7 for YYYY-MM-DD

const appendMonthSeparator = (v: string) => (v.length === 2 ? `${v}/` : v);

export interface FormatProps {
  accept: RegExp,
  shouldMask: (v: string) => boolean,
  format: (str: string) => string,
  append?: (str: string) => string,
}

export const dateFormatProps: FormatProps = {
  accept: /\d+/g,
  shouldMask: v => v.length >= 10,
  format: formatDateWithAppend,
  append: appendDateSeparator,
};

export const monthFormatProps: FormatProps = {
  accept: /\d+/g,
  shouldMask: v => v.length >= 7,
  format: formatMonthWithAppend,
  append: appendMonthSeparator,
}

export const yearFormatProps: FormatProps = {
  accept: /\d+/g,
  shouldMask: v => v.length >= 4,
  format: formatYear,
};