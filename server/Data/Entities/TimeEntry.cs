using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities
{
	public class TimeEntry
	{
		[Key]
		public int TimeEntryId { get; set; }

		public DateOnly Date { get; set; }

		public decimal Hours { get; set; }

		public string Comment { get; set; } = string.Empty;

		[ForeignKey(nameof(UserId))]
		public virtual User User { get; set; }
		public int UserId { get; set; }

		[ForeignKey(nameof(ProjectId))]
		public virtual Project Project { get; set; }
		public int ProjectId { get; set; }
	}
}
