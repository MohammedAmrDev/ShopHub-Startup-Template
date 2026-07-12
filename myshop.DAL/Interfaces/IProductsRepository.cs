using myshop.Models.Entities;
using System.Linq.Expressions;

namespace myshop.DAL.Interfaces
{
	public interface IProductsRepository : IGenericRepository<Product>
	{
		Task<List<Product>> GetAllAsync(params Expression<Func<Product, object>>[] includes);
		Task<Product?> GetByIdAsync(int id, params Expression<Func<Product, object>>[] includes);
	}
}