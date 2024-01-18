import {
  ProjectCreateModel,
  ProjectModelResult,
  ProjectsModelResult
} from '../types/projects';
import { fetcher } from './fetcher';

export const getProject = async (
  projectId: number
): Promise<ProjectModelResult> =>
  await fetcher(`Project/Single?userId=1&projectId=${projectId}`);

export const getProjects = async (): Promise<ProjectsModelResult> =>
  await fetcher('Project/List?userId=1');

export const createProject = async (
  model: ProjectCreateModel
): Promise<ProjectModelResult> => {
  return await fetcher('Project/Create', 'POST', model);
};
