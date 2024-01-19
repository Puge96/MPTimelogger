using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
	public class User
	{
		[Key]
		public int UserId { get; set; }

		[StringLength(255)]
		public string Name { get; set; }

		[ForeignKey(nameof(CompanyId))]
		public virtual Company Company { get; set; }

		public int CompanyId { get; set; }
	}
}
