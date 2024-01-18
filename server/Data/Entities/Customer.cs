using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }

        [StringLength(255)]
        public string Name { get; set; }

        [ForeignKey(nameof(CompanyId))]
        public virtual Company Company { get; set; }

        public int CompanyId { get; set; }
    }
}
