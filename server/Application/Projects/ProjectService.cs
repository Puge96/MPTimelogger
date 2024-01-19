using Application.Customers;
using Application.Projects.Models;
using Application.Shared;
using Application.TimeEntries.Models;
using Application.Users;
using Data;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Projects
{
	public class ProjectService
	{
		private readonly DataContext dataContext;
		private readonly UserService userService;
		private readonly CustomerService customerService;

		public ProjectService(DataContext dataContext, UserService userService, CustomerService customerService)
		{
			this.dataContext = dataContext;
			this.userService = userService;
			this.customerService = customerService;
		}

		public async Task<ProjectModelResult> Single(int userId, int projectId, CancellationToken cancellationToken)
		{
			var result = new ProjectModelResult();

			var userResult = await userService.Single(userId, cancellationToken);
			if (!userResult.IsValid)
			{
				result.Errors.AddRange(userResult.Errors);
				return result;
			}

			var project = await dataContext.Project.FirstOrDefaultAsync(x => x.ProjectId == projectId && x.CompanyId == userResult.User!.CompanyId, cancellationToken);
			if (project == null)
			{
				result.Errors.Add("Project does not exist or you don't have the required access to view it.");
				return result;
			}

			result.Project = new ProjectDTO(project);
			return result;
		}

		public async Task<ProjectsModelResult> List(int userId, CancellationToken cancellationToken)
		{
			var result = new ProjectsModelResult();

			var userResult = await userService.Single(userId, cancellationToken);
			if (!userResult.IsValid)
			{
				result.Errors.AddRange(userResult.Errors);
				return result;
			}

			var projects = await dataContext.Project.Where(x => x.CompanyId == userResult.User!.CompanyId).ToListAsync(cancellationToken);
			if (projects != null)
			{
				result.Projects = projects.Select(x => new ProjectDTO(x)).ToList();
			}

			return result;
		}

		public async Task<ProjectModelResult> Create(ProjectCreateModel model, CancellationToken cancellationToken)
		{
			var result = new ProjectModelResult();

			if (model.EndDate < model.StartDate)
			{
				result.Errors.Add("Start date must be lower than or equal to end date.");
				return result;
			}

			var userResult = await userService.Single(model.UserId, cancellationToken);
			if (!userResult.IsValid)
			{
				result.Errors.AddRange(userResult.Errors);
				return result;
			}

			if (userResult.User!.CompanyId != model.CompanyId)
			{
				result.Errors.Add("You do not have access to the provided company.");
				return result;
			}

			var customerResult = await customerService.List(model.UserId, cancellationToken);
			if (!customerResult.IsValid)
			{
				result.Errors.AddRange(customerResult.Errors);
				return result;
			}

			if (!customerResult.Customers.Any(x => x.CustomerId == model.CustomerId))
			{
				result.Errors.Add("Customer does not exist");
				return result;
			}

			if (await dataContext.Project.AnyAsync(x => x.CompanyId == model.CompanyId && x.CustomerId == model.CustomerId && x.Name == model.Name))
			{
				result.Errors.Add($"Customer already has a project named {model.Name}");
				return result;
			}

			if (result.IsValid)
			{
				var newProject = new Project
				{
					Name = model.Name,
					CompanyId = model.CompanyId,
					CustomerId = model.CustomerId,
					Status = model.Status,
					StartDate = model.StartDate,
					EndDate = model.EndDate
				};

				await dataContext.Project.AddAsync(newProject, cancellationToken);
				await dataContext.SaveChangesAsync(cancellationToken);

				result.Project = new ProjectDTO(newProject);
			}
			return result;
		}

		public async Task<ProjectModelResult> Update(ProjectUpdateModel model, CancellationToken cancellationToken)
		{
			var result = new ProjectModelResult();

			if (model.EndDate < model.StartDate)
			{
				result.Errors.Add("Start date must be lower than or equal to end date.");
				return result;
			}

			var userResult = await userService.Single(model.UserId, cancellationToken);
			if (!userResult.IsValid)
			{
				result.Errors.AddRange(userResult.Errors);
				return result;
			}

			var project = await dataContext.Project.AsTracking().FirstOrDefaultAsync(x => x.ProjectId == model.ProjectId && x.CompanyId == userResult.User!.CompanyId, cancellationToken);
			if (project == null)
			{
				result.Errors.Add("Project does not exist or you don't have the required access to view it.");
				return result;
			}

			if (result.IsValid)
			{
				project.Name = model.Name;
				project.Status = model.Status;
				project.StartDate = model.StartDate;
				project.EndDate = model.EndDate;

				await dataContext.SaveChangesAsync(cancellationToken);
				result.Project = new ProjectDTO(project);
			}

			return result;
		}

		public async Task<BaseJsonResult> Delete(int userId, int projectId, CancellationToken cancellationToken)
		{
			var result = new TimeEntryModelResult();

			var userResult = await userService.Single(userId, cancellationToken);
			if (!userResult.IsValid)
			{
				result.Errors.AddRange(userResult.Errors);
				return result;
			}

			var project = await dataContext.Project.FirstOrDefaultAsync(x => x.ProjectId == projectId && x.CompanyId == userResult.User!.CompanyId, cancellationToken);
			if (project == null)
			{
				// Don't throw error if project does not exist as it might just have been deleted by another user.
				return result;
			}

			if (project.Status != ProjectStatus.Open)
			{
				result.Errors.Add("You can only delete open projects.");
				return result;
			}

			if (await dataContext.TimeEntry.AnyAsync(x => x.ProjectId == projectId, cancellationToken))
			{
				result.Errors.Add("You cannot delete a project that contains time entry data");
				return result;
			}

			if (result.IsValid)
			{
				dataContext.Remove(project);
				await dataContext.SaveChangesAsync(cancellationToken);
			}

			return result;
		}
	}
}
