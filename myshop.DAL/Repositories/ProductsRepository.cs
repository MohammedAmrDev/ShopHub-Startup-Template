using Microsoft.EntityFrameworkCore;
using myshop.DAL.Data;
using myshop.DAL.Interfaces;
using myshop.Models.Entities;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace myshop.DAL.Repositories
{
	internal class ProductsRepository : GenericRepository<Product>, IProductsRepository
	{
		private readonly ApplicationDbContext _context;
		public ProductsRepository(ApplicationDbContext context) : base(context) => _context = context;

		public async Task<Product?> GetByIdAsync(int id, params Expression<Func<Product, object>>[] includes)
		{
			IQueryable<Product> query = _context.Products;

			foreach (var include in includes)
			{
				query = query.Include(include);
			}

			return await query.FirstOrDefaultAsync(p => p.Id == id);
		}
	}
}
