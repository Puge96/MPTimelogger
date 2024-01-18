using Application.Customers;
using Application.Customers.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace API.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	[AllowAnonymous]
	public class CustomerController : ControllerBase
	{
        private readonly CustomerService customerService;

        public CustomerController(CustomerService customerService)
		{
            this.customerService = customerService;
        }

		[HttpGet]
        public async Task<ActionResult<CustomerModelResult>> Single(int userId, int customerId, CancellationToken cancellationToken)
		{
			var result = await customerService.Single(userId, customerId, cancellationToken);

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
		public async Task<ActionResult<CustomersModelResult>> List(int userId, CancellationToken cancellationToken)
		{
            var result = await customerService.List(userId, cancellationToken);

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
