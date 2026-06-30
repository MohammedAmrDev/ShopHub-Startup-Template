using myshop.Models.Entities;

namespace myshop.BLL.Interfaces
{
	public interface ICategoriesService
	{
		Task<List<Category>> GetCategoriesAsync();
		Task CreateCategoryAsync(Category category);
		Task<Category?> GetCategoryByIdAsync(int id);
		Task UpdateCategoryAsync(Category category);
		Task DeleteCategoryAsync(int id);
	}
}
