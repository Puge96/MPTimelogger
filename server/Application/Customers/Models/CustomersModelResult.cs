using Application.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Customers.Models
{
    public class CustomersModelResult : BaseJsonResult
    {
        public List<CustomerDTO> Customers { get; set; } = new List<CustomerDTO>();
    }
}
