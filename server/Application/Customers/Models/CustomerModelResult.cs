using Application.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Customers.Models
{
	public class CustomerModelResult : BaseJsonResult
	{
		public CustomerDTO? Customer { get; set; }
	}
}
