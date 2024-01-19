using Application.Users.Models;
using Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users
{
	public class UserService
	{
		private readonly DataContext dataContext;

		public UserService(DataContext dataContext)
		{
			this.dataContext = dataContext;
		}

		public async Task<UserModelResult> Single(int userId, CancellationToken cancellationToken)
		{
			var result = new UserModelResult();

			var user = await dataContext.User.FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

			if (user == null)
			{
				result.Errors.Add("User does not exist or you do not have the required access.");
				return result;
			}

			result.User = new UserDTO(user);

			return result;
		}

		public async Task<UsersModelResult> List(int userId, CancellationToken cancellationToken)
		{
			var result = new UsersModelResult();

			var user = await dataContext.User.FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

			if (user == null)
			{
				result.Errors.Add("User does not exist or you do not have the required access.");
				return result;
			}

			var users = await dataContext.User.Where(x => x.CompanyId == user.CompanyId).ToListAsync(cancellationToken);
			if (users != null)
			{
				result.Users = users.Select(x => new UserDTO(x)).ToList();
			}

			return result;
		}
	}
}
