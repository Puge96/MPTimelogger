import { BaseJsonResult } from './baseJsonResult';

export type TimeEntryDTO = {
  timeEntryId: number;
  date: string;
  hours: number;
  comment: string;
  userId: string;
  projectId: number;
};

export type TimeEntryModelResult = BaseJsonResult & {
  timeEntry: TimeEntryDTO;
};

export type TimeEntriesModelResult = BaseJsonResult & {
  timeEntries: TimeEntryDTO[];
};

export type TimeEntryCreateModel = {
  userId: number;
  projectId: number;
  date: string;
  comment: string;
  hours: number;
};

export type TimeEntryCreateFormValues = {
  date: Date;
  comment: string;
  hours: number;
};

export const TimeEntryCreateFormFields = {
  projectId: 'projectId',
  date: 'date',
  comment: 'comment',
  hours: 'hours'
};
