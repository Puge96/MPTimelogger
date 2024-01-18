import { useController } from 'react-hook-form';
import { DropdownOption } from '../../types/dropdownOption';
import { Listbox, Transition } from '@headlessui/react';
import { Fragment } from 'react';
import { cs } from '../../helpers/cssHelper';

export const DropdownField = ({
  label,
  name,
  placeholder,
  disabled = false,
  errorMessage,
  errors,
  control,
  options
}: {
  label?: string;
  name: string;
  placeholder: string;
  disabled?: boolean;
  errorMessage: string;
  errors: any;
  control: any;
  options: DropdownOption[];
}) => {
  const {
    field: { value, onChange, ref }
  } = useController({
    name,
    control,
    rules: { validate: (v) => v !== null }
  });

  const selected = options.find((o) => o.value?.key === value?.key);

  // "No options" message
  if (options.length === 0) {
    options = [
      {
        disabled: true,
        label: 'There are no options.',
        value: {
          key: ''
        }
      }
    ];
  }

  return (
    <div>
      {label && (
        <label
          htmlFor={name}
          className="mb-0.5 block text-sm leading-6 text-gray-900"
        >
          {label}
        </label>
      )}

      <Listbox onChange={onChange} value={value} disabled={disabled}>
        {({ open }) => (
          <div className="relative w-full">
            <Listbox.Button
              ref={ref}
              className="relative w-full cursor-default rounded-md bg-white py-2 pl-2.5 pr-10 text-left leading-6 text-gray-900 ring-1 ring-inset ring-gray-300 focus:outline-none focus:ring-2 focus:ring-indigo-500"
            >
              <span
                className={cs(
                  'block truncate',
                  disabled ? 'text-gray-400' : ''
                )}
              >
                {value === null ? placeholder : `${selected?.label}`}
              </span>
            </Listbox.Button>

            <Transition
              show={open}
              as={Fragment}
              leave="transition ease-in duration-100"
              leaveFrom="opacity-100"
              leaveTo="opacity-0"
            >
              <Listbox.Options className="absolute z-10 mt-1 max-h-60 w-full overflow-auto rounded-md bg-white py-1 text-base shadow-lg ring-1 ring-black ring-opacity-5 focus:outline-none">
                {options.map((option) => (
                  <Listbox.Option
                    className={({ active }) =>
                      cs(
                        active ? 'bg-indigo-500 text-white' : 'text-gray-900',
                        'relative cursor-default select-none py-2 pl-3 pr-9'
                      )
                    }
                    disabled={option.disabled}
                    value={option.value}
                    key={option.label}
                  >
                    {({ selected }) => (
                      <>
                        <span
                          className={cs(
                            selected ? 'font-semibold' : 'font-normal',
                            'block truncate'
                          )}
                        >
                          {option.label}
                        </span>
                      </>
                    )}
                  </Listbox.Option>
                ))}
              </Listbox.Options>
            </Transition>

            {errors[name] && (
              <p className="mt-1 text-sm text-red-600">{errorMessage}</p>
            )}
          </div>
        )}
      </Listbox>
    </div>
  );
};
