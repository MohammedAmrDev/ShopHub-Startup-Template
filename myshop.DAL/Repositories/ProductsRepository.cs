using Microsoft.EntityFrameworkCore;
using myshop.DAL.Data;
using myshop.DAL.Interfaces;
using myshop.Models.Entities;

namespace myshop.DAL.Repositories
{
	internal class ProductsRepository : IProductsRepository
	{
		private readonly ApplicationDbContext _context;
		public ProductsRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<List<Product>> GetAllAsync()
		{
			return await _context.Products.AsNoTracking().Include(x => x.Category).ToListAsync();
		}

		public async Task<Product?> GetByIdAsync(int id)
		{
			return await _context.Products.FindAsync(id);
		}

		public async Task CreateAsync(Product product)
		{
			await _context.Products.AddAsync(product);
		}

		public async Task UpdateAsync(Product newProduct)
		{
			Product? product = await _context.Products.FindAsync(newProduct.Id);
			if (product == null)
				return;
			_context.Entry(product).CurrentValues.SetValues(newProduct); // Instead of automapper or manual mapping
		}

		public async Task<int> DeleteAsync(int id)
		{
			// ExecuteDeleteAsync() is an EF Core 7+ bulk operation — it executes the DELETE SQL immediately and directly on the database, bypassing EF's change tracker entirely.
			return await _context.Products.Where(p => p.Id == id).ExecuteDeleteAsync(); // returns rows affected
		}
	}
}
