using Microsoft.EntityFrameworkCore;
using myshop.DAL.Data;
using myshop.DAL.Interfaces;
using myshop.Models.Entities;

namespace myshop.DAL.Repositories
{
	internal class CategoriesRepository : ICategoriesRepository
	{
		private readonly ApplicationDbContext _context;
		public CategoriesRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<List<Category>> GetAllAsync()
		{
			return await _context.Categories.AsNoTracking().ToListAsync();
		}

		public async Task<Category?> GetByIdAsync(int id)
		{
			return await _context.Categories.FindAsync(id);
		}

		public async Task CreateAsync(Category category)
		{
			await _context.Categories.AddAsync(category);
		}

		public async Task UpdateAsync(Category newCategory)
		{
			Category? category = await _context.Categories.FindAsync(newCategory.Id);
			if (category == null)
				return;
			_context.Entry(category).CurrentValues.SetValues(newCategory);
		}

		public async Task DeleteAsync(int id)
		{
			await _context.Categories.Where(p => p.Id == id).ExecuteDeleteAsync();
		}
	}
}
