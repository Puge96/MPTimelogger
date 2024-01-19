using System.ComponentModel.DataAnnotations;

namespace Application.Customers.Models
{
	public class CustomerCreateModel
	{
		public int UserId { get; set; }

		public int CompanyId { get; set; }

		[StringLength(255)]
		public string Name { get; set; } = string.Empty;
	}
}
