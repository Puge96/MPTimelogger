using Application.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TimeEntries.Models
{
    public class TimeEntriesModelResult : BaseJsonResult
    {
        public List<TimeEntryDTO> TimeEntries { get; set; } = new List<TimeEntryDTO>();
    }
}
