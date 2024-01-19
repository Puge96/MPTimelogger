using Data.Entities;

namespace Application.Users.Models
{
	public class UserDTO
	{
		public UserDTO(User model)
		{
			this.UserId = model.UserId;
			this.Name = model.Name;
			this.CompanyId = model.CompanyId;
		}

		public int UserId { get; set; }

		public string Name { get; set; }

		public int CompanyId { get; set; }
	}
}
