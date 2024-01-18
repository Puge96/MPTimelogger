using Application.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Projects.Models
{
    public class ProjectModelResult : BaseJsonResult
    {
        public ProjectDTO? Project { get; set; }
    }
}
