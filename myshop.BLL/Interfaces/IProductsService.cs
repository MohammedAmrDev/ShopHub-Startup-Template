using myshop.Models.DTOs;
using myshop.Models.ViewModels;

namespace myshop.BLL.Interfaces
{
	public interface IProductsService
	{
		Task<List<ProductResponse>> GetAllProducts();
		Task CreateProduct(ProductViewModel productViewModel);
		Task<ProductResponse?> GetProductById(int id);
		Task UpdateProduct(ProductViewModel productViewModel);
		Task<bool> DeleteProductAsync(int id);
	}
}