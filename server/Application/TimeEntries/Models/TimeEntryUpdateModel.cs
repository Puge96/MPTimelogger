using Application.Shared;
using System.ComponentModel.DataAnnotations;

namespace Application.TimeEntries.Models
{
	public class TimeEntryUpdateModel
	{
		public int UserId { get; set; }

		public int TimeEntryId { get; set; }

		public decimal Hours { get; set; }

		[StringLength(255)]
		public string Comment { get; set; } = string.Empty;
	}
}
