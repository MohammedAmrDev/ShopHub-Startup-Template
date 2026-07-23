using myshop.BLL.Interfaces;
using myshop.DAL.Interfaces;
using myshop.Models.ViewModels;

namespace myshop.BLL.Services
{
	public class UserManagementService : IUserManagementService
	{
		private readonly IUserManagementRepository _usersRepo;

		public UserManagementService(IUserManagementRepository usersRepo)
		{
			_usersRepo = usersRepo;
		}

		public async Task<(List<UserManagementViewModel> data, int recordsTotal, int recordsFiltered)> GetUsersAsync(string? search, string? orderBy, string? orderDir, int? start, int? length)
		{
			var data = await _usersRepo.GetAllForDataTableAsync(
				string.IsNullOrEmpty(search) ? null : u => u.UserName.ToLower().Contains(search.ToLower()),
				orderBy,
				orderDir,
				start ?? 0,
				length ?? 5
			);

			return (data, await _usersRepo.GetCountAsync(), data.Count());
		}
	}
}
