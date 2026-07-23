using myshop.Models.DTOs;
using myshop.Models.ViewModels;

namespace myshop.BLL.Interfaces
{
	public interface ICategoriesService
	{
		Task<(List<CategoryResponse> data, int recordsTotal, int recordsFiltered)> GetCategoriesAsync(string? search, string? orderBy, string? orderDir, int? start, int? length);
		Task<List<CategoryResponse>> GetCategoriesAsync();
		Task<CategoryResponse?> GetCategoryByIdAsync(int id);
		Task CreateCategoryAsync(CategoryViewModel category);
		Task UpdateCategoryAsync(CategoryViewModel category);
		Task<bool> DeleteCategoryAsync(int id);
	}
}
