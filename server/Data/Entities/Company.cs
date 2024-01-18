using System.ComponentModel.DataAnnotations;

namespace Data.Entities
{
    public class Company
    {
        [Key]
        public int CompanyId { get; set; }

        [StringLength(255)]
        public string Name { get; set; }
    }
}
