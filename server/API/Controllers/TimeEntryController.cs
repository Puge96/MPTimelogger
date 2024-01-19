using Application.Shared;
using Application.TimeEntries;
using Application.TimeEntries.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace API.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	[AllowAnonymous]
	public class TimeEntryController : ControllerBase
	{
		private readonly TimeEntryService timeEntryService;

		public TimeEntryController(TimeEntryService timeEntryService)
		{
			this.timeEntryService = timeEntryService;
		}

		[HttpGet]
		public async Task<ActionResult<TimeEntryModelResult>> Single(int userId, int timeEntryId, CancellationToken cancellationToken)
		{
			var result = await timeEntryService.Single(userId, timeEntryId, cancellationToken);

			if (result.IsValid)
			{
				return Ok(result);
			}
			else
			{
				return BadRequest(result);
			}
		}

		[HttpGet]
		public async Task<ActionResult<TimeEntriesModelResult>> List(int userId, int? projectId, CancellationToken cancellationToken)
		{
			var result = await timeEntryService.List(userId, projectId, cancellationToken);

			if (result.IsValid)
			{
				return Ok(result);
			}
			else
			{
				return BadRequest(result);
			}
		}

		[HttpPost]
		public async Task<ActionResult<TimeEntryModelResult>> Create(TimeEntryCreateModel model, CancellationToken cancellationToken)
		{
			var result = await timeEntryService.Create(model, cancellationToken);

			if (result.IsValid)
			{
				return Ok(result);
			}
			else
			{
				return BadRequest(result);
			}
		}

		[HttpPut]
		public async Task<ActionResult<TimeEntryModelResult>> Update(TimeEntryUpdateModel model, CancellationToken cancellationToken)
		{
			var result = await timeEntryService.Update(model, cancellationToken);

			if (result.IsValid)
			{
				return Ok(result);
			}
			else
			{
				return BadRequest(result);
			}
		}

		[HttpDelete]
		public async Task<ActionResult<BaseJsonResult>> Delete(int userId, int timeEntryId, CancellationToken cancellationToken)
		{
			var result = await timeEntryService.Delete(userId, timeEntryId, cancellationToken);

			if (result.IsValid)
			{
				return Ok(result);
			}
			else
			{
				return BadRequest(result);
			}
		}
	}
}
