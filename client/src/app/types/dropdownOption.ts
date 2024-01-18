export {};

export type DropdownOption = {
  disabled?: boolean;
  label: string;
  value: DropdownOptionValue | null;
};

export type DropdownOptionValue = {
  key: string;
};
