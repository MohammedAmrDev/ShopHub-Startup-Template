using myshop.BLL.Interfaces;
using myshop.DAL.Interfaces;
using myshop.Models.Entities;

namespace myshop.BLL.Services
{
	public class CategoriesService : ICategoriesService
	{
		private readonly IUnitOfWork _uow;
		public CategoriesService(IUnitOfWork uow)
		{
			_uow = uow;
		}

		public async Task<List<Category>> GetCategoriesAsync()
		{
			return await _uow.Categories.GetAllAsync();
		}
		public async Task<Category?> GetCategoryByIdAsync(int id)
		{
			return await _uow.Categories.GetByIdAsync(id);
		}

		public async Task CreateCategoryAsync(Category category)
		{
			await _uow.Categories.CreateAsync(category);
			await _uow.SaveChangesAsync();
		}

		public async Task UpdateCategoryAsync(Category category)
		{
			await _uow.Categories.UpdateAsync(category);
			await _uow.SaveChangesAsync();
		}

		public async Task DeleteCategoryAsync(int id)
		{
			await _uow.Categories.DeleteAsync(id);
			await _uow.SaveChangesAsync();
		}
	}
}
