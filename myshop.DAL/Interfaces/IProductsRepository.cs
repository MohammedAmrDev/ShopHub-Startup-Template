using myshop.Models.Entities;

namespace myshop.DAL.Interfaces
{
	public interface IProductsRepository
	{
		Task<List<Product>> GetAllAsync();
		Task<Product?> GetByIdAsync(int id);
		Task CreateAsync(Product product);
		Task UpdateAsync(Product newProduct);
		Task<int> DeleteAsync(int id);
	}
}
