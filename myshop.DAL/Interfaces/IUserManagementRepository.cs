using myshop.Models.IdentityEntities;
using myshop.Models.ViewModels;
using System.Linq.Expressions;

namespace myshop.DAL.Interfaces
{
	public interface IUserManagementRepository
	{
		Task<int> GetCountAsync();
		Task<List<UserManagementViewModel>> GetAllForDataTableAsync(
			Expression<Func<ApplicationUser, bool>>? filter,
			string? orderBy,
			string? orderDir,
			int start,
			int length
		);
	}
}
