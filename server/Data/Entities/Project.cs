using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities
{
	public class Project
	{
		[Key]
		public int ProjectId { get; set; }

		[StringLength(255)]
		public string Name { get; set; }

		public ProjectStatus Status { get; set; }

		public DateOnly StartDate { get; set; }

		public DateOnly EndDate { get; set; }

		[ForeignKey(nameof(CustomerId))]
		public virtual Customer Customer { get; set; }

		public int CustomerId { get; set; }

		[ForeignKey(nameof(CompanyId))]
		public virtual Company Company { get; set; }

		public int CompanyId { get; set; }
	}

	public enum ProjectStatus
	{
		Open,
		Closed
	}
}
