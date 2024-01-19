using Application.Users;
using Application.Users.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace API.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	[AllowAnonymous]
	public class UserController : ControllerBase
	{
		private readonly UserService userService;

		public UserController(UserService userService)
		{
			this.userService = userService;
		}

		[HttpGet]
		public async Task<ActionResult<UserModelResult>> Single(int userId, CancellationToken cancellationToken)
		{
			var result = await userService.Single(userId, cancellationToken);

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
		public async Task<ActionResult<UsersModelResult>> List(int userId, CancellationToken cancellationToken)
		{
			var result = await userService.List(userId, cancellationToken);

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
