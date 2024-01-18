import { BaseJsonResult } from './baseJsonResult';
import { DropdownOptionValue } from './dropdownOption';

export type ProjectDTO = {
  projectId: number;
  name: string;
  status: ProjectStatus;
  startDate: string;
  endDate: string;
  customerId: number;
  companyId: number;
};

export type ProjectModelResult = BaseJsonResult & {
  project: ProjectDTO;
};

export type ProjectsModelResult = BaseJsonResult & {
  projects: ProjectDTO[];
};

export type ProjectCreateModel = {
  userId: number;
  name: string;
  status: ProjectStatus;
  startDate: string;
  endDate: string;
  customerId: number;
  companyId: number;
};

export type ProjectCreateFormValues = {
  name: string;
  status: DropdownOptionValue | null;
  startDate: Date;
  endDate: Date;
  customerId: DropdownOptionValue | null;
};

export const ProjectCreateFormFields = {
  name: 'name',
  status: 'status',
  startDate: 'startDate',
  endDate: 'endDate',
  customerId: 'customerId'
};

export type ProjectUpdateModel = {
  userId: number;
  projectId: number;
  name: string;
  status: ProjectStatus;
  startDate: string;
  endDate: string;
};
// 0 = Open
// 1 = Closed
export type ProjectStatus = 0 | 1;
