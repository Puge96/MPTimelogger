using Application.Shared;
using System.ComponentModel.DataAnnotations;

namespace Application.TimeEntries.Models
{
	public class TimeEntryCreateModel
	{
		public int UserId { get; set; }

		public int ProjectId { get; set; }

		public DateOnly Date { get; set; }

		public decimal Hours { get; set; }

		[StringLength(255)]
		public string Comment { get; set; } = string.Empty;
	}
}
