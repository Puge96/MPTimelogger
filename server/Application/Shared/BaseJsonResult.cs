using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Shared
{
    public class BaseJsonResult
    {
        public List<string> Errors { get; set; } = new();
        public bool IsValid
        {
            get
            {
                return !Errors.Any();
            }
        }
    }
}
