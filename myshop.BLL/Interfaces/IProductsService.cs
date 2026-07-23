using myshop.Models.DTOs;
using myshop.Models.ViewModels;

namespace myshop.BLL.Interfaces
{
	public interface IProductsService
	{
		Task<(List<ProductResponse> data, int recordsTotal, int recordsFiltered)> GetAllProducts(string? search, string? orderBy, string? orderDir, int? start, int? length);
		Task CreateProduct(ProductViewModel productViewModel);
		Task<ProductResponse?> GetProductById(int id);
		Task UpdateProduct(ProductViewModel productViewModel);
		Task<bool> DeleteProductAsync(int id);
	}
}