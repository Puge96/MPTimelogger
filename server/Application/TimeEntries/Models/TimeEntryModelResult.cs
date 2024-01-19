using Application.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TimeEntries.Models
{
	public class TimeEntryModelResult : BaseJsonResult
	{
		public TimeEntryDTO? TimeEntry { get; set; }
	}
}
