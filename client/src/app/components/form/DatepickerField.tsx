import { Fragment } from 'react';
import { useController } from 'react-hook-form';
import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";
import { cs } from '../../helpers/cssHelper';

export const DatePickerField = ({
  label,
  name,
  errorMessage,
  errors,
  control
}: {
  label?: string;
  name: string;
  errorMessage: string;
  errors: any;
  control: any;
}) => {
  const {
    field: { value, onChange, ref }
  } = useController({
    name,
    control,
    rules: { validate: (v) => v !== null }
  });

  return (
    <Fragment>
      <div>
        {label && (
          <label
            htmlFor={name}
            className="mb-0.5 block text-sm leading-6 text-gray-900"
          >
            {label}
          </label>
        )}
      </div>

      <DatePicker wrapperClassName="dp-full-width" className="p-2 dp-full-width rounded-md border" ref={ref} onChange={onChange} value={value} selected={value}></DatePicker>

      {errors[name] && (
        <p className="mt-1 text-sm text-red-600">{errorMessage}</p>
      )}
    </Fragment>
  );
}