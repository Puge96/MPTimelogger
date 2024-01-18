using Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Customers.Models
{
    public class CustomerDTO
    {
        public CustomerDTO(Customer model)
        {
            this.CustomerId = model.CustomerId;
            this.CompanyId = model.CompanyId;
            this.Name = model.Name;
        }

        public int CustomerId { get; set; }

        public int CompanyId { get; set; }

        public string Name { get; set; } = string.Empty;
    }
}
