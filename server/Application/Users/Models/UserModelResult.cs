using Application.Shared;

namespace Application.Users.Models
{
	public class UserModelResult : BaseJsonResult
	{
		public UserDTO? User { get; set; }
	}
}
