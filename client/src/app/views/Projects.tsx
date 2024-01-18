import React, { Fragment, useEffect, useState } from 'react';
import { useForm } from 'react-hook-form';
import { createProject, getProjects } from '../api/projectApi';
import {
  ProjectCreateModel,
  ProjectDTO,
  ProjectCreateFormFields,
  ProjectCreateFormValues,
  ProjectStatus
} from '../types/projects';
import { Link } from 'react-router-dom';
import { CustomerDTO } from '../types/customers';
import { getCustomers } from '../api/customerApi';
import ModalDialog from '../components/ModalDialog';
import { TextField } from '../components/form/TextField';
import { DropdownField } from '../components/form/DropdownField';
import { DropdownOption } from '../types/dropdownOption';
import { DatePickerField } from '../components/form/DatepickerField';
import { DateToStringAPIFormat } from '../helpers/dateHelper';

export default function Projects() {
  const [projects, setProjects] = useState<ProjectDTO[]>([]);
  const [customers, setCustomers] = useState<CustomerDTO[]>([]);

  // Simple data fetcher
  const fetchData = async () => {
    var results = await Promise.all([
      await getProjects(),
      await getCustomers()
    ]);

    if (results[0].isValid && results[1].isValid) {
      setProjects(results[0].projects);
      setCustomers(results[1].customers);
    } else {
      setProjects([]);
      setCustomers([]);
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

  const projectStatusOptions: DropdownOption[] = [
    {
      label: 'Open',
      value: { key: '0' }
    },
    {
      label: 'Closed',
      value: { key: '1' }
    }
  ];

  return (
    <Fragment>
      <div className="flex items-center my-6">
        <CreateNewProject
          dataReloader={reloadData}
          customers={customers}
          projectStatusOptions={projectStatusOptions}
        />
      </div>

      <ProjectTable
        projects={projects}
        customers={customers}
        projectStatusOptions={projectStatusOptions}
      />
    </Fragment>
  );
}

const CreateNewProject = ({
  dataReloader,
  customers,
  projectStatusOptions
}: {
  dataReloader: () => void;
  customers: CustomerDTO[];
  projectStatusOptions: DropdownOption[];
}) => {
  const [showDialog, setShowDialog] = useState(false);
  const closeDialog = () => setShowDialog(false);
  const {
    control,
    register,
    handleSubmit,
    formState: { errors }
  } = useForm<ProjectCreateFormValues>({
    defaultValues: {
      name: undefined,
      customerId: null,
      startDate: undefined,
      endDate: undefined,
      status: null
    }
  });

  const onSubmit = async (formData: ProjectCreateFormValues) => {
    const model: ProjectCreateModel = {
      companyId: 1,
      userId: 1,
      name: formData.name,
      customerId: +(formData.customerId?.key || 0),
      status: +(formData.status?.key || 0) as ProjectStatus,
      startDate: DateToStringAPIFormat(formData.startDate),
      endDate: DateToStringAPIFormat(formData.endDate)
    };

    var result = await createProject(model);

    if (result.isValid) {
      await dataReloader();
    }

    closeDialog();
  };

  const customerOptions: DropdownOption[] = customers.map((x) => {
    return {
      label: x.name,
      value: { key: x.customerId.toString() }
    };
  });

  return (
    <Fragment>
      <button
        onClick={() => setShowDialog(true)}
        className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded"
      >
        New project
      </button>

      <ModalDialog
        title={'Create new project'}
        confirmButtonText={'Create'}
        onConfirm={handleSubmit(onSubmit)}
        onCancel={closeDialog}
        isOpen={showDialog}
      >
        <form onSubmit={handleSubmit(onSubmit)}>
          <div className="mb-5">
            <TextField
              autoFocus
              label={'Name'}
              name={ProjectCreateFormFields.name}
              register={register}
              registerOptions={{
                required: 'Name is required'
              }}
              maxLength={255}
              errors={errors}
            />
          </div>
          <div className="mb-5">
            <DropdownField
              label={'Customer'}
              name={ProjectCreateFormFields.customerId}
              control={control}
              placeholder={'Select customer'}
              errors={errors}
              errorMessage={'Customer is required'}
              options={customerOptions}
            />
          </div>
          <div className="mb-5">
            <DropdownField
              label={'Status'}
              name={ProjectCreateFormFields.status}
              control={control}
              placeholder={'Select project status'}
              errors={errors}
              errorMessage={'Project status is required'}
              options={projectStatusOptions}
            />
          </div>
          <div className="mb-5">
            <DatePickerField
              label={'Start date'}
              name={ProjectCreateFormFields.startDate}
              control={control}
              errorMessage={"Start date is required"}
              errors={errors}
            />
          </div>
          <div className="mb-5">
            <DatePickerField
              label={'End date'}
              name={ProjectCreateFormFields.endDate}
              control={control}
              errorMessage={"End date is required"}
              errors={errors}
            />
          </div>
        </form>
      </ModalDialog>
    </Fragment>
  );
};

const ProjectTable = ({
  projects,
  customers,
  projectStatusOptions
}: {
  projects: ProjectDTO[];
  customers: CustomerDTO[];
  projectStatusOptions: DropdownOption[];
}) => {
  const [sortOrder, setSortOrder] = useState<{
    field: keyof ProjectDTO;
    direction: 'asc' | 'desc';
  }>({ field: 'name', direction: 'asc' });

  const handleSort = (field: any) => {
    setSortOrder((prevSortOrder) => ({
      field,
      direction: prevSortOrder.direction === 'asc' ? 'desc' : 'asc'
    }));
  };

  const sortedProjects = [...projects].sort((a, b) => {
    let aValue: string | number | Date;
    let bValue: string | number | Date;
    if (sortOrder.field === 'startDate' || sortOrder.field === 'endDate') {
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

  if (!Array.isArray(projects) || projects.length === 0) {
    return <div>There are no projects yet...</div>;
  } else {
    return (
      <table className="table-fixed w-full">
        <thead className="bg-gray-200">
          <tr>
            <th
              onClick={() => handleSort('projectId')}
              className="border px-4 py-2 w-12 cursor-pointer"
            >
              #{' '}
              {sortOrder.field === 'projectId' &&
                sortOrder.direction === 'asc' &&
                '↑'}
              {sortOrder.field === 'projectId' &&
                sortOrder.direction === 'desc' &&
                '↓'}
            </th>
            <th
              onClick={() => handleSort('name')}
              className="border px-4 py-2 cursor-pointer"
            >
              Name{' '}
              {sortOrder.field === 'name' &&
                sortOrder.direction === 'asc' &&
                '↑'}
              {sortOrder.field === 'name' &&
                sortOrder.direction === 'desc' &&
                '↓'}
            </th>
            <th
              onClick={() => handleSort('customerId')}
              className="border px-4 py-2 cursor-pointer"
            >
              Customer{' '}
              {sortOrder.field === 'customerId' &&
                sortOrder.direction === 'asc' &&
                '↑'}
              {sortOrder.field === 'customerId' &&
                sortOrder.direction === 'desc' &&
                '↓'}
            </th>
            <th
              onClick={() => handleSort('startDate')}
              className="border px-4 py-2 cursor-pointer"
            >
              Start date{' '}
              {sortOrder.field === 'startDate' &&
                sortOrder.direction === 'asc' &&
                '↑'}
              {sortOrder.field === 'startDate' &&
                sortOrder.direction === 'desc' &&
                '↓'}
            </th>
            <th
              onClick={() => handleSort('endDate')}
              className="border px-4 py-2 cursor-pointer"
            >
              End date{' '}
              {sortOrder.field === 'endDate' &&
                sortOrder.direction === 'asc' &&
                '↑'}
              {sortOrder.field === 'endDate' &&
                sortOrder.direction === 'desc' &&
                '↓'}
            </th>
            <th
              onClick={() => handleSort('status')}
              className="border px-4 py-2 cursor-pointer"
            >
              Status{' '}
              {sortOrder.field === 'status' &&
                sortOrder.direction === 'asc' &&
                '↑'}
              {sortOrder.field === 'status' &&
                sortOrder.direction === 'desc' &&
                '↓'}
            </th>
          </tr>
        </thead>
        <tbody>
          {sortedProjects.map((project, index) => (
            <tr key={index}>
              <td className="border px-4 py-2 w-12">{project.projectId}</td>
              <td className="border px-4 py-2">
                {
                  <Link to={'/project/' + project.projectId}>
                    <span className="hover:text-blue-500">
                      {project.name} &#x2197;
                    </span>
                  </Link>
                }
              </td>
              <td className="border px-4 py-2">
                {customers.find((x) => x.customerId === project.customerId)
                  ?.name ?? 'Unknown'}
              </td>
              <td className="border px-4 py-2">
                {new Date(project.startDate).toLocaleDateString('en-US')}
              </td>
              <td className="border px-4 py-2">
                {new Date(project.endDate).toLocaleDateString('en-US')}
              </td>
              <td className="border px-4 py-2">
                {projectStatusOptions.find(
                  (x) => x.value!.key === project.status.toString()
                )?.label ?? 'Unknown'}
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    );
  }
};
