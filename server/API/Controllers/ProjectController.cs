using Application.Projects;
using Application.Projects.Models;
using Application.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace API.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	[AllowAnonymous]
	public class ProjectController : ControllerBase
	{
        private readonly ProjectService projectService;

        public ProjectController(ProjectService projectService)
		{
            this.projectService = projectService;
        }

		[HttpGet]
        public async Task<ActionResult<ProjectModelResult>> Single(int userId, int projectId, CancellationToken cancellationToken)
		{
			var result = await projectService.Single(userId, projectId, cancellationToken);

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
		public async Task<ActionResult<ProjectsModelResult>> List(int userId, CancellationToken cancellationToken)
		{
            var result = await projectService.List(userId, cancellationToken);

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
		public async Task<ActionResult<ProjectModelResult>> Create(ProjectCreateModel model, CancellationToken cancellationToken)
		{
            var result = await projectService.Create(model, cancellationToken);

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
		public async Task<ActionResult<ProjectModelResult>> Update(ProjectUpdateModel model, CancellationToken cancellationToken)
		{
			var result = await projectService.Update(model, cancellationToken);

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
		public async Task<ActionResult<BaseJsonResult>> Delete(int userId, int projectId, CancellationToken cancellationToken)
		{
			var result = await projectService.Delete(userId, projectId, cancellationToken);

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
