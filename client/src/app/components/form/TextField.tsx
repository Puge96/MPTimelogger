import { cs } from '../../helpers/cssHelper';

export const TextField = ({
  label,
  name,
  register,
  registerOptions,
  errors,
  maxLength = 255,
  type = 'text',
  autoFocus
}: {
  label?: string;
  name: string;
  register: any;
  registerOptions: any;
  errors: any;
  maxLength?: number;
  type?: string;
  autoFocus?: boolean
}) => (
  <div>
    {label && (
      <label htmlFor={name} className="mb-0.5 block text-sm text-gray-900">
        {label}
      </label>
    )}
    <div
      className={cs(
        'relative flex gap-x-1 rounded-md border',
        errors[name] ? 'border-red-500' : 'border-gray-300'
      )}
    >
      <input
        id={name}
        className="w-full rounded-md p-2 focus:border-indigo-500 focus:ring-indigo-500"
        maxLength={maxLength}
        type={type}
        autoFocus={autoFocus}
        {...register(name, registerOptions)}
      />
    </div>

    {errors[name] && (
      <p className="mt-1 text-sm text-red-600">{errors[name]?.message}</p>
    )}
  </div>
);
