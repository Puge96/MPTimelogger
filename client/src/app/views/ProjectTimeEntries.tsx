import React, { Fragment, useEffect, useState } from 'react';
import { useParams } from 'react-router';
import { getProject } from '../api/projectApi';
import { ProjectDTO } from '../types/projects';
import { createTimeEntry, getTimeEntries } from '../api/timeEntryApi';
import {
  TimeEntryCreateFormFields,
  TimeEntryCreateFormValues,
  TimeEntryCreateModel,
  TimeEntryDTO
} from '../types/timeEntries';
import { useForm } from 'react-hook-form';
import ModalDialog from '../components/ModalDialog';
import { TextField } from '../components/form/TextField';
import { DatePickerField } from '../components/form/DatepickerField';
import { DateToStringAPIFormat } from '../helpers/dateHelper';

export default function ProjectTimeEntries() {
  const { projectId } = useParams();
  const [project, setProject] = useState<ProjectDTO | null>(null);
  const [timeEntries, setTimeEntries] = useState<TimeEntryDTO[]>([]);

  // Simple data fetcher
  const fetchData = async () => {
    if (projectId != null) {
      var result = await Promise.all([
        await getProject(+projectId),
        await getTimeEntries(+projectId)
      ]);

      if (result[0].isValid && result[1].isValid) {
        setProject(result[0].project);
        setTimeEntries(result[1].timeEntries);
      } else {
        setProject(null);
        setTimeEntries([]);
      }
    }
  };

  // Initial data loader
  useEffect(() => {
    fetchData();
  }, []);

  //simple data reloader
  const reloadData = async () => {
    await fetchData();
  };

  return (
    <Fragment>
      <div className="mt-5">
        {project != null && (
          <h2 className="text-xl font-semibold">Time entries for <span className="font-bold">{project.name}</span></h2>
          )}
      </div>

      <div className="mt-5">
        <div className="flex items-center my-6">
          <CreateNewTimeEntry dataReloader={reloadData} />
        </div>
        <TimeEntriesTable timeEntries={timeEntries} />
      </div>
    </Fragment>
  );
}

const CreateNewTimeEntry = ({ dataReloader }: { dataReloader: () => void }) => {
  const { projectId } = useParams();
  const [showDialog, setShowDialog] = useState(false);
  const closeDialog = () => setShowDialog(false);
  const {
    control,
    register,
    handleSubmit,
    formState: { errors }
  } = useForm<TimeEntryCreateFormValues>({
    defaultValues: {
      date: undefined,
      comment: undefined,
      hours: undefined
    }
  });

  const onSubmit = async (formData: TimeEntryCreateFormValues) => {
    const model: TimeEntryCreateModel = {
      userId: 1,
      projectId: +(projectId || 0),
      date: DateToStringAPIFormat(formData.date),
      comment: formData.comment,
      hours: formData.hours
    };

    var result = await createTimeEntry(model);

    if (result.isValid) {
      await dataReloader();
    }

    closeDialog();
  };

  return (
    <Fragment>
      <button
        onClick={() => setShowDialog(true)}
        className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded"
      >
        New time entry
      </button>

      <ModalDialog
        title={'Create new time entry'}
        confirmButtonText={'Create'}
        onConfirm={handleSubmit(onSubmit)}
        onCancel={closeDialog}
        isOpen={showDialog}
      >
        <form onSubmit={handleSubmit(onSubmit)}>
          <div className="mb-5">
            <DatePickerField
              label={'Date'}
              name={TimeEntryCreateFormFields.date}
              control={control}
              errors={errors}
              errorMessage={'Date is required'}
            />
          </div>
          <div className="mb-5">
            <TextField
              label={'Comment'}
              name={TimeEntryCreateFormFields.comment}
              register={register}
              registerOptions={{
                required: 'Comment is required'
              }}
              maxLength={255}
              errors={errors}
            />
          </div>
          <div className="mb-5">
            <TextField
              label={'Hours (0.5 = 30m)'}
              name={TimeEntryCreateFormFields.hours}
              type={'number'}
              register={register}
              registerOptions={{
                required: "Hours is required",
                min: {
                  value: 0.5,
                  message: 'Please enter a value between 0.5 and 24.'
                },
                max: {
                  value: 24,
                  message: 'Please enter a value between 0.5 and 24.'
                },
              }}
              maxLength={255}
              errors={errors}
            />
          </div>
        </form>
      </ModalDialog>
    </Fragment>
  );
};

const TimeEntriesTable = ({ timeEntries }: { timeEntries: TimeEntryDTO[] }) => {
  const [sortOrder, setSortOrder] = useState<{
    field: keyof TimeEntryDTO;
    direction: 'asc' | 'desc';
  }>({ field: 'date', direction: 'desc' });

  const handleSort = (field: any) => {
    setSortOrder((prevSortOrder) => ({
      field,
      direction: prevSortOrder.direction === 'asc' ? 'desc' : 'asc'
    }));
  };

  const sortedTimeEntries = [...timeEntries].sort((a, b) => {
    let aValue: string | number | Date;
    let bValue: string | number | Date;
    if (sortOrder.field === 'date') {
      aValue = new Date(a[sortOrder.field]);
      bValue = new Date(b[sortOrder.field]);
    } else {
      aValue = a[sortOrder.field];
      bValue = b[sortOrder.field];
    }

    if (sortOrder.direction === 'asc') {
      return aValue > bValue ? 1 : -1;
    } else {
      return aValue < bValue ? 1 : -1;
    }
  });

  const totalHours = timeEntries.reduce((total, entry) => total + entry.hours, 0);
  if (!Array.isArray(timeEntries) || timeEntries.length === 0) {
    return <div>There are no time entries for this project yet...</div>;
  } else {
    return (
      <table className="table-fixed w-full">
        <thead className="bg-gray-200">
          <tr>
            <th
              onClick={() => handleSort('date')}
              className="border px-4 py-2 cursor-pointer"
            >
              Date{' '}
              {sortOrder.field === 'date' &&
                sortOrder.direction === 'asc' &&
                '↑'}
              {sortOrder.field === 'date' &&
                sortOrder.direction === 'desc' &&
                '↓'}
            </th>
            <th
              onClick={() => handleSort('comment')}
              className="border px-4 py-2 cursor-pointer"
            >
              Comment{' '}
              {sortOrder.field === 'comment' &&
                sortOrder.direction === 'asc' &&
                '↑'}
              {sortOrder.field === 'comment' &&
                sortOrder.direction === 'desc' &&
                '↓'}
            </th>
            <th
              onClick={() => handleSort('hours')}
              className="border px-4 py-2 cursor-pointer"
            >
              Hours{' '}
              {sortOrder.field === 'hours' &&
                sortOrder.direction === 'asc' &&
                '↑'}
              {sortOrder.field === 'hours' &&
                sortOrder.direction === 'desc' &&
                '↓'}
            </th>
          </tr>
        </thead>
        <tbody>
          {sortedTimeEntries.map((timeEntry, index) => (
            <tr key={index}>
              <td className="border px-4 py-2">
                {new Date(timeEntry.date).toLocaleDateString('en-US')}
              </td>
              <td className="border px-4 py-2">{timeEntry.comment}</td>
              <td className="border px-4 py-2 text-right">{timeEntry.hours.toFixed(2)}</td>
            </tr>
          ))}
          <tr className="bg-indigo-400 text-white">
            <td className="border px-4 py-2 font-bold">Total</td>
            <td className="border px-4 py-2"></td>
            <td className="border px-4 py-2 text-right font-bold">{totalHours.toFixed(2)}</td>
          </tr>
        </tbody>
      </table>
    );
  }
};
