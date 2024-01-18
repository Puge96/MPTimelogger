export function cs(...classes: (string | string[])[]) {
  return classes.reduce((acc: string, val) => {
    if (Array.isArray(val)) {
      acc += ' ' + val.filter(Boolean).join(' ');
    } else {
      acc += ' ' + val;
    }

    return acc;
  }, '');
}
