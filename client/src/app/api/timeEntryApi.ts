import {
  TimeEntriesModelResult,
  TimeEntryCreateModel,
  TimeEntryModelResult
} from '../types/timeEntries';
import { fetcher } from './fetcher';

export const getTimeEntry = async (
  timeEntryId: number
): Promise<TimeEntryModelResult> =>
  await fetcher(`TimeEntry/Single?userId=1&timeEntryId=${timeEntryId}`);

export const getTimeEntries = async (
  projectId?: number
): Promise<TimeEntriesModelResult> => {
  let url = 'TimeEntry/List?userId=1';
  if (projectId != null) {
    url += `&projectId=${projectId}`;
  }

  return await fetcher(url);
};

export const createTimeEntry = async (
  model: TimeEntryCreateModel
): Promise<TimeEntryModelResult> => {
  return await fetcher('TimeEntry/Create', 'POST', model);
};
