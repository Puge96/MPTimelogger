using Application.Projects;
using Application.Projects.Models;
using Application.TimeEntries;
using Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Application.Test
{
	[Collection("MyTestCollection")]
	public class Tests
	{
		private readonly DataContext dataContext;
		private readonly ProjectService projectService;
		private readonly TimeEntryService timeEntryService;

		public Tests(DataContext dataContext, ProjectService projectService, TimeEntryService timeEntryService)
		{
			this.dataContext = dataContext;
			// Reset database and test data
			this.dataContext.Database.EnsureDeleted();
			this.dataContext.Database.EnsureCreated();
			this.dataContext.SeedDatabase();
			this.projectService = projectService;
			this.timeEntryService = timeEntryService;
		}

		#region Project service tests
		[Fact]
		public async Task Project_Create_Valid_ReturnsSuccess()
		{
			var projectModel = new ProjectCreateModel
			{
				UserId = 1,
				CompanyId = 1,
				CustomerId = 1,
				Name = "xUnit Test Project",
				Status = Data.Entities.ProjectStatus.Open,
				StartDate = DateOnly.FromDateTime(DateTime.UtcNow),
				EndDate = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(2))
			};

			var projectResult = await projectService.Create(projectModel, CancellationToken.None);

			Assert.True(projectResult.IsValid);
			Assert.NotNull(projectResult.Project);
		}

		[Fact]
		public async Task Project_Create_InvalidDate_ReturnsError()
		{

			var invalidModel = new ProjectCreateModel
			{
				UserId = 1,
				CompanyId = 1,
				CustomerId = 1,
				Name = "xUnit Test Project",
				StartDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)), // Start date exceeding end date
				EndDate = DateOnly.FromDateTime(DateTime.UtcNow)
			};

			// Act
			var result = await projectService.Create(invalidModel, CancellationToken.None);

			// Assert
			Assert.False(result.IsValid);
			Assert.Contains("Start date must be lower than or equal to end date.", result.Errors);
		}

		[Fact]
		public async Task Project_Delete_ClosedProject_ReturnsError()
		{

			var testProject = await dataContext.Project.FirstOrDefaultAsync(CancellationToken.None);

			var updateResult = await projectService.Update(new ProjectUpdateModel
			{
				UserId = 1,
				ProjectId = testProject.ProjectId,
				Name = testProject.Name,
				Status = Data.Entities.ProjectStatus.Closed,
				StartDate = testProject.StartDate,
				EndDate = testProject.EndDate,
			}, CancellationToken.None);

			Assert.True(updateResult.IsValid);
			Assert.NotNull(updateResult.Project);

			var deleteResult = await projectService.Delete(1, testProject.ProjectId, CancellationToken.None);

			Assert.False(deleteResult.IsValid);
			Assert.Contains("You can only delete open projects.", deleteResult.Errors);
		}

		[Fact]
		public async Task Project_Delete_ProjectWithTimeEntry_ReturnsError()
		{
			var testProject = await dataContext.Project.FirstOrDefaultAsync(CancellationToken.None);

			var timeEntryResult = await timeEntryService.Create(new TimeEntries.Models.TimeEntryCreateModel
			{
				UserId = 1,
				ProjectId = testProject.ProjectId,
				Date = DateOnly.FromDateTime(DateTime.UtcNow),
				Comment = "Test comment",
				Hours = 1.5m
			}, CancellationToken.None);

			Assert.True(timeEntryResult.IsValid);

			var deleteResult = await projectService.Delete(1, testProject.ProjectId, CancellationToken.None);

			Assert.False(deleteResult.IsValid);
			Assert.Contains("You cannot delete a project that contains time entry data", deleteResult.Errors);
		}
		#endregion

		#region Time entry service tests
		[Fact]
		public async Task TimeEntry_Create_Valid_ReturnsSuccess()
		{

			var testProject = await dataContext.Project.FirstOrDefaultAsync(CancellationToken.None);

			var timeEntryResult = await timeEntryService.Create(new TimeEntries.Models.TimeEntryCreateModel
			{
				UserId = 1,
				ProjectId = testProject.ProjectId,
				Date = DateOnly.FromDateTime(DateTime.UtcNow),
				Comment = "Test comment",
				Hours = 1.5m
			}, CancellationToken.None);

			Assert.True(timeEntryResult.IsValid);
			Assert.NotNull(timeEntryResult.TimeEntry);
		}

		[Fact]
		public async Task TimeEntry_Create_InvalidHours_ReturnsError()
		{
			var testProject = await dataContext.Project.FirstOrDefaultAsync(CancellationToken.None);
			var timeEntryResult1 = await timeEntryService.Create(new TimeEntries.Models.TimeEntryCreateModel
			{
				UserId = 1,
				ProjectId = testProject.ProjectId,
				Date = DateOnly.FromDateTime(DateTime.UtcNow),
				Comment = "Test comment",
				Hours = 0.49m
			}, CancellationToken.None);

			Assert.False(timeEntryResult1.IsValid);
			Assert.Contains("Hours must be between 0.5 and 24 hours", timeEntryResult1.Errors);

			var timeEntryResult2 = await timeEntryService.Create(new TimeEntries.Models.TimeEntryCreateModel
			{
				UserId = 1,
				ProjectId = testProject.ProjectId,
				Date = DateOnly.FromDateTime(DateTime.UtcNow),
				Comment = "Test comment",
				Hours = 24.01m
			}, CancellationToken.None);

			Assert.False(timeEntryResult2.IsValid);
			Assert.Contains("Hours must be between 0.5 and 24 hours", timeEntryResult2.Errors);
		}

		[Fact]
		public async Task TimeEntry_Create_TooManyHours_ReturnsError()
		{
			var testProject = await dataContext.Project.FirstOrDefaultAsync(CancellationToken.None);
			var timeEntryResult1 = await timeEntryService.Create(new TimeEntries.Models.TimeEntryCreateModel
			{
				UserId = 1,
				ProjectId = testProject.ProjectId,
				Date = DateOnly.FromDateTime(DateTime.UtcNow),
				Comment = "Test comment",
				Hours = 10m
			}, CancellationToken.None);

			Assert.True(timeEntryResult1.IsValid);
			Assert.NotNull(timeEntryResult1.TimeEntry);

			var secondProjectResult = await projectService.Create(new ProjectCreateModel
			{
				UserId = 1,
				CompanyId = 1,
				CustomerId = 1,
				Name = "Test project v2",
				Status = Data.Entities.ProjectStatus.Open,
				StartDate = DateOnly.FromDateTime(DateTime.UtcNow),
				EndDate = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(5))
			}, CancellationToken.None);

			Assert.True(secondProjectResult.IsValid);
			Assert.NotNull(secondProjectResult.Project);

			var timeEntryResult2 = await timeEntryService.Create(new TimeEntries.Models.TimeEntryCreateModel
			{
				UserId = 1,
				ProjectId = secondProjectResult.Project.ProjectId,
				Date = DateOnly.FromDateTime(DateTime.UtcNow),
				Comment = "Test comment",
				Hours = 15m
			}, CancellationToken.None);

			Assert.False(timeEntryResult2.IsValid);
			Assert.Contains("Unable to create time registration as number of hours for the day would exceed 24 hours.", timeEntryResult2.Errors);
		}

		[Fact]
		public async Task TimeEntry_Update_InvalidHours_ReturnsError()
		{
			var testProject = await dataContext.Project.FirstOrDefaultAsync(CancellationToken.None);
			var timeEntryResult = await timeEntryService.Create(new TimeEntries.Models.TimeEntryCreateModel
			{
				UserId = 1,
				ProjectId = testProject.ProjectId,
				Date = DateOnly.FromDateTime(DateTime.UtcNow),
				Comment = "Test comment",
				Hours = 0.5m
			}, CancellationToken.None);

			Assert.True(timeEntryResult.IsValid);

			var updateTimeEntryResult = await timeEntryService.Update(new TimeEntries.Models.TimeEntryUpdateModel
			{
				UserId = 1,
				TimeEntryId = timeEntryResult.TimeEntry!.TimeEntryId,
				Comment = "Test comment",
				Hours = 24.01m,

			}, CancellationToken.None);

			Assert.False(updateTimeEntryResult.IsValid);
			Assert.Contains("Hours must be between 0.5 and 24 hours", updateTimeEntryResult.Errors);
		}

		[Fact]
		public async Task TimeEntry_Update_TooManyHours_ReturnsError()
		{
			var testProject = await dataContext.Project.FirstOrDefaultAsync(CancellationToken.None);
			var timeEntryResult1 = await timeEntryService.Create(new TimeEntries.Models.TimeEntryCreateModel
			{
				UserId = 1,
				ProjectId = testProject.ProjectId,
				Date = DateOnly.FromDateTime(DateTime.UtcNow),
				Comment = "Test comment",
				Hours = 10m
			}, CancellationToken.None);

			Assert.True(timeEntryResult1.IsValid);
			Assert.NotNull(timeEntryResult1.TimeEntry);

			var secondProjectResult = await projectService.Create(new ProjectCreateModel
			{
				UserId = 1,
				CompanyId = 1,
				CustomerId = 1,
				Name = "Test project v2",
				Status = Data.Entities.ProjectStatus.Open,
				StartDate = DateOnly.FromDateTime(DateTime.UtcNow),
				EndDate = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(5))
			}, CancellationToken.None);

			Assert.True(secondProjectResult.IsValid);
			Assert.NotNull(secondProjectResult.Project);

			var timeEntryResult2 = await timeEntryService.Create(new TimeEntries.Models.TimeEntryCreateModel
			{
				UserId = 1,
				ProjectId = secondProjectResult.Project.ProjectId,
				Date = DateOnly.FromDateTime(DateTime.UtcNow),
				Comment = "Test comment",
				Hours = 14m
			}, CancellationToken.None);

			Assert.True(timeEntryResult2.IsValid);
			Assert.NotNull(timeEntryResult2.TimeEntry);

			var updateResult = await timeEntryService.Update(new TimeEntries.Models.TimeEntryUpdateModel
			{
				UserId = 1,
				TimeEntryId = timeEntryResult1.TimeEntry.TimeEntryId,
				Comment = "Test comment",
				Hours = 10.01m
			}, CancellationToken.None);

			Assert.False(updateResult.IsValid);
			Assert.Contains("Unable to update time registration as number of hours for the day would exceed 24 hours.", updateResult.Errors);
		}
		#endregion
	}
}
