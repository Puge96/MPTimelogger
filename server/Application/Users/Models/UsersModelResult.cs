using Application.Shared;

namespace Application.Users.Models
{
    public class UsersModelResult : BaseJsonResult
    {
        public List<UserDTO> Users { get; set; } = new List<UserDTO>();
    }
}
