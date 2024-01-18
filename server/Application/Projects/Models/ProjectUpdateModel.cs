using Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Projects.Models
{
    public class ProjectUpdateModel
    {
        public int UserId { get; set; }

        public int ProjectId { get; set; }

        [StringLength(255)]
        public string Name { get; set; } = string.Empty;

        public ProjectStatus Status { get; set; }

        public DateOnly StartDate { get; set; }

        public DateOnly EndDate { get; set; }
    }
}
