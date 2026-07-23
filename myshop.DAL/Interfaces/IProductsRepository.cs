using myshop.Models.Entities;
using System.Linq.Expressions;

namespace myshop.DAL.Interfaces
{
	public interface IProductsRepository : IGenericRepository<Product>
	{
		Task<Product?> GetByIdAsync(int id, params Expression<Func<Product, object>>[] includes);
	}
}