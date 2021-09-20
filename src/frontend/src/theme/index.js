import { extendTheme } from "@chakra-ui/react";

import core from "./core";
import styles from "./styles";

const overrides = {
  ...core,
  styles,
};

export default extendTheme(overrides);