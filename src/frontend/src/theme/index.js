import { extendTheme } from "@chakra-ui/react";

import components from "./components";
import core from "./core";
import styles from "./styles";

const overrides = {
  components,
  ...core,
  styles,
};

export default extendTheme(overrides);