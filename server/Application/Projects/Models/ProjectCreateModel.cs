using Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace Application.Projects.Models
{
	public class ProjectCreateModel
	{
		public int UserId { get; set; }

		[StringLength(255)]
		public string Name { get; set; } = string.Empty;

		public int CustomerId { get; set; }

		public int CompanyId { get; set; }

		public ProjectStatus Status { get; set; }

		public DateOnly StartDate { get; set; }

		public DateOnly EndDate { get; set; }
	}
}
