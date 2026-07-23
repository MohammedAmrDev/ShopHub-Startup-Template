using myshop.Models.ViewModels;

namespace myshop.BLL.Interfaces
{
	public interface IUserManagementService
	{
		Task<(List<UserManagementViewModel> data, int recordsTotal, int recordsFiltered)> GetUsersAsync(string? search, string? orderBy, string? orderDir, int? start, int? length);
	}
}
