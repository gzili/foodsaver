export function absPath(path: string | null, replaceBacklashes = false) {
  if (path != null) {
    let normalizedPath = replaceBacklashes ? path.replaceAll('\\', '/') : path;
    return '/' + normalizedPath;
  } else {
    return undefined;
  }
}