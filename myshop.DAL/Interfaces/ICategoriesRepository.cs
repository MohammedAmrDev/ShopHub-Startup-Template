using myshop.Models.Entities;

namespace myshop.DAL.Interfaces
{
	public interface ICategoriesRepository
	{
		Task<List<Category>> GetAllAsync();
		Task<Category?> GetByIdAsync(int id);
		Task CreateAsync(Category category);
		Task UpdateAsync(Category category);
		Task DeleteAsync(int id);
	}
}
