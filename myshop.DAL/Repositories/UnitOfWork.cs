using myshop.DAL.Data;
using myshop.DAL.Interfaces;

namespace myshop.DAL.Repositories
{
	public class UnitOfWork : IUnitOfWork
	{
		public ICategoriesRepository Categories { get; }
		public IProductsRepository Products { get; }
		private readonly ApplicationDbContext _context;

		public UnitOfWork(ApplicationDbContext context)
		{
			_context = context;
			Categories = new CategoriesRepository(_context);
			Products = new ProductsRepository(_context);
		}

		public async Task<int> SaveChangesAsync()
			=> await _context.SaveChangesAsync();
	}
}
