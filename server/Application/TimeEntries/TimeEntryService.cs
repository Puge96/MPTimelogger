using Application.Projects;
using Application.Shared;
using Application.TimeEntries.Models;
using Application.Users;
using Data;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TimeEntries
{
    public class TimeEntryService
    {
        private readonly DataContext dataContext;
        private readonly UserService userService;
        private readonly ProjectService projectService;

        public TimeEntryService(DataContext dataContext, UserService userService, ProjectService projectService)
        {
            this.dataContext = dataContext;
            this.userService = userService;
            this.projectService = projectService;
        }

        public async Task<TimeEntryModelResult> Single(int userId, int timeEntryId, CancellationToken cancellationToken)
        {
            var result = new TimeEntryModelResult();

            var userResult = await userService.Single(userId, cancellationToken);
            if (!userResult.IsValid)
            {
                result.Errors.AddRange(userResult.Errors);
                return result;
            }

            var timeEntry = await dataContext.TimeEntry.FirstOrDefaultAsync(x => x.TimeEntryId == timeEntryId, cancellationToken);
            if (timeEntry == null)
            {
                result.Errors.Add("Time entry does not exist or you don't have the required access to view it.");
                return result;
            }

            result.TimeEntry = new TimeEntryDTO(timeEntry);
            return result;
        }

        public async Task<TimeEntriesModelResult> List(int userId, int? projectId, CancellationToken cancellationToken)
        {
            var result = new TimeEntriesModelResult();

            var userResult = await userService.Single(userId, cancellationToken);
            if (!userResult.IsValid)
            {
                result.Errors.AddRange(userResult.Errors);
                return result;
            }

            // Fetch time entries.
            var timeEntries = await dataContext.TimeEntry
                .Where(x => x.UserId == userId && 
                    (x.ProjectId == projectId || projectId == null)
                ).ToListAsync(cancellationToken);

            if (timeEntries != null)
            {
                result.TimeEntries.AddRange(timeEntries.Select(x => new TimeEntryDTO(x)));
            }

            return result;
        }

        public async Task<TimeEntryModelResult> Create(TimeEntryCreateModel model, CancellationToken cancellationToken)
        {
            var result = new TimeEntryModelResult();

            if (model.Hours < 0.5m || model.Hours > 24m)
            {
                result.Errors.Add("Hours must be between 0.5 and 24 hours");
                return result;
            }

            var sameDayUserTimeEntries = await dataContext.TimeEntry.Where(x => x.UserId == model.UserId && x.Date == model.Date).ToListAsync(cancellationToken);
            if (sameDayUserTimeEntries.Any())
            {
                var existingHours = sameDayUserTimeEntries.Sum(x => x.Hours);
                if (existingHours + model.Hours > 24m)
                {
                    result.Errors.Add("Unable to create time registration as number of hours for the day would exceed 24 hours.");
                    return result;
                }
            }

            var projectResult = await projectService.Single(model.UserId, model.ProjectId, cancellationToken);
            if (!projectResult.IsValid)
            {
                result.Errors.AddRange(projectResult.Errors);
                return result;
            }

            if (await dataContext.TimeEntry.AnyAsync(x => x.ProjectId == model.ProjectId && x.Date == model.Date && x.UserId == model.UserId, cancellationToken))
            {
                result.Errors.Add("You already have time entry for this project on this date. Please update the record instead");
                return result;
            }

            if (result.IsValid)
            {
                var timeEntry = new TimeEntry
                {
                    UserId = model.UserId,
                    Date = model.Date,
                    ProjectId = model.ProjectId,
                    Hours = model.Hours,
                    Comment = model.Comment
                };

                await dataContext.TimeEntry.AddAsync(timeEntry, cancellationToken);
                await dataContext.SaveChangesAsync(cancellationToken);
                result.TimeEntry = new TimeEntryDTO(timeEntry);
            }

            return result;
        }

        public async Task<TimeEntryModelResult> Update(TimeEntryUpdateModel model, CancellationToken cancellationToken)
        {
            var result = new TimeEntryModelResult();

            if (model.Hours < 0.5m || model.Hours > 24m)
            {
                result.Errors.Add("Hours must be between 0.5 and 24 hours");
                return result;
            }

            var timeEntry = await dataContext.TimeEntry.AsTracking().FirstOrDefaultAsync(x => x.TimeEntryId == model.TimeEntryId && x.UserId == model.UserId, cancellationToken);
            if (timeEntry == null)
            {
                result.Errors.Add("Time entry does not exist");
                return result;
            }

            var sameDayUserTimeEntries = await dataContext.TimeEntry.Where(x => x.UserId == model.UserId && x.Date == timeEntry.Date && x.TimeEntryId != timeEntry.TimeEntryId).ToListAsync(cancellationToken);
            if (sameDayUserTimeEntries.Any())
            {
                var existingHours = sameDayUserTimeEntries.Sum(x => x.Hours);
                if (existingHours + model.Hours > 24m)
                {
                    result.Errors.Add("Unable to update time registration as number of hours for the day would exceed 24 hours.");
                    return result;
                }
            }

            if (result.IsValid)
            {
                timeEntry.Hours = model.Hours;
                timeEntry.Comment = model.Comment;

                await dataContext.SaveChangesAsync(cancellationToken);
                result.TimeEntry = new TimeEntryDTO(timeEntry);
            }

            return result;
        }

        public async Task<BaseJsonResult> Delete(int userId, int timeEntryId, CancellationToken cancellationToken)
        {
            var result = new BaseJsonResult();

            var timeEntry = await dataContext.TimeEntry.FirstOrDefaultAsync(x => x.TimeEntryId == timeEntryId, cancellationToken);

            if (timeEntry == null)
            {
                return result;
            }

            var projectResult = await projectService.Single(userId, timeEntry.ProjectId, cancellationToken);
            if (!projectResult.IsValid)
            {
                result.Errors.AddRange(projectResult.Errors);
                return result;
            }

            if (projectResult.Project!.Status != ProjectStatus.Open)
            {
                result.Errors.Add("You cannot delete a time entry on a closed project");
            }

            if (timeEntry.UserId != userId)
            {
                result.Errors.Add("You are only allowed to delete your own time entry");
            }

            if (result.IsValid)
            {
                dataContext.Remove(timeEntry);
                await dataContext.SaveChangesAsync(cancellationToken);
            }

            return result;
        }
    }
}
