using Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TimeEntries.Models
{
	public class TimeEntryDTO
	{
		public TimeEntryDTO(TimeEntry model)
		{
			this.TimeEntryId = model.TimeEntryId;
			this.Date = model.Date;
			this.Hours = model.Hours;
			this.Comment = model.Comment;
			this.UserId = model.UserId;
			this.ProjectId = model.ProjectId;
		}

		public int TimeEntryId { get; set; }

		public DateOnly Date { get; set; }

		public decimal Hours { get; set; }

		public string Comment { get; set; }

		public int UserId { get; set; }

		public int ProjectId { get; set; }
	}
}
