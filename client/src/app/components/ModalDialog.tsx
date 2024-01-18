import React from 'react';
import { Fragment, useRef } from 'react';
import { Dialog, Transition } from '@headlessui/react';
import { cs } from '../helpers/cssHelper';

export default function ModalDialog({
  title,
  confirmButtonText,
  cancelButtonText,
  theme = 'information',
  onConfirm,
  onCancel,
  isOpen,
  width = 'sm',
  children
}: {
  title: string;
  confirmButtonText?: string;
  cancelButtonText?: string;
  theme?: 'information' | 'danger';
  onConfirm?: () => void;
  onCancel: () => void;
  isOpen: boolean;
  width?: 'sm' | 'lg';
  children: React.ReactNode;
}) {
  const cancelButtonRef = useRef(null);
  return (
    <Transition.Root show={isOpen} as={Fragment}>
      <Dialog
        as="div"
        className="relative z-10"
        initialFocus={cancelButtonRef}
        onClose={onCancel}
      >
        <Transition.Child
          as={Fragment}
          enter="ease-out duration-300"
          enterFrom="opacity-0"
          enterTo="opacity-100"
          leave="ease-in duration-200"
          leaveFrom="opacity-100"
          leaveTo="opacity-0"
        >
          <div className="fixed inset-0 bg-gray-500 bg-opacity-75 transition-opacity" />
        </Transition.Child>

        <div className="fixed inset-0 z-10 overflow-y-auto">
          <div className="flex min-h-full items-end justify-center p-4 text-center sm:items-center sm:p-0">
            <Transition.Child
              as="div"
              className={cs(
                'relative w-full ',
                width === 'sm' ? 'max-w-xl' : 'max-w-3xl'
              )}
              enter="ease-out duration-300"
              enterFrom="opacity-0 translate-y-4 sm:translate-y-0 sm:scale-95"
              enterTo="opacity-100 translate-y-0 sm:scale-100"
              leave="ease-in duration-200"
              leaveFrom="opacity-100 translate-y-0 sm:scale-100"
              leaveTo="opacity-0 translate-y-4 sm:translate-y-0 sm:scale-95"
            >
              {/* Dialog body */}
              <Dialog.Panel className="z-0 transform rounded-lg bg-white text-left shadow-xl transition-all">
                <div className="p-8">
                  {/* Dialog title */}
                  <Dialog.Title
                    as="h3"
                    className="text-lg font-semibold leading-6 text-gray-900"
                  >
                    {title}
                  </Dialog.Title>

                  {/* Dialog content */}
                  <div className="mt-2">{children}</div>
                </div>

                {/* Dialog buttons */}
                <div className="flex flex-row-reverse rounded-b-lg bg-gray-50 px-6 py-4">
                  {onConfirm != null && (
                    <button
                      type="button"
                      className={cs(
                        'inline-flex w-[100px] justify-center rounded-md px-3 py-2 text-white shadow-sm sm:ml-3',
                        theme === 'information'
                          ? 'bg-indigo-500 hover:bg-indigo-600/90 '
                          : 'bg-red-500 hover:bg-red-500/90'
                      )}
                      onClick={onConfirm}
                    >
                      {confirmButtonText ?? 'Confirm'}
                    </button>
                  )}

                  <button
                    type="button"
                    className="mt-3 inline-flex w-[100px] justify-center rounded-md bg-white px-3 py-2 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 hover:bg-gray-50 sm:mt-0"
                    onClick={onCancel}
                    ref={cancelButtonRef}
                  >
                    {cancelButtonText ?? 'Cancel'}
                  </button>
                </div>
              </Dialog.Panel>
            </Transition.Child>
          </div>
        </div>
      </Dialog>
    </Transition.Root>
  );
}
