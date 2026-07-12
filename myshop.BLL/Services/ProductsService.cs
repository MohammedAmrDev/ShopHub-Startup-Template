using AutoMapper;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using myshop.BLL.Interfaces;
using myshop.DAL.Interfaces;
using myshop.Models.DTOs;
using myshop.Models.Entities;
using myshop.Models.ViewModels;

namespace myshop.BLL.Services
{
	public class ProductsService : IProductsService
	{
		private readonly IUnitOfWork _uow;
		private readonly IMapper _mapper;
		private readonly IImageService _imageService;

		public ProductsService(IUnitOfWork uow, IMapper mapper, IImageService imageService)
		{
			_uow = uow;
			_mapper = mapper;
			_imageService = imageService;
		}

		public async Task<List<ProductResponse>> GetAllProducts()
		{
			List<Product> products = await _uow.Products.GetAllAsync(x => x.Category);
			return products.Select(_mapper.Map<ProductResponse>).ToList();
		}

		public async Task CreateProduct(ProductViewModel productViewModel)
		{
			Product product = _mapper.Map<Product>(productViewModel);
			string? ImageURL = _imageService.UploadImage(productViewModel.ImageFile); // ImageFile will not be null here because of the custom validation
			if (ImageURL != null)
				product.Img = ImageURL;
			await _uow.Products.CreateAsync(product);
			await _uow.SaveChangesAsync();
		}

		public async Task<ProductResponse?> GetProductById(int id)
		{
			Product? product = await _uow.Products.GetByIdAsync(id, x => x.Category);
			if (product == null)
				return null;
			return _mapper.Map<ProductResponse>(product);
		}

		public async Task UpdateProduct(ProductViewModel productViewModel)
		{
			if (productViewModel.ImageFile != null && productViewModel.ImageURL != null)
			{
				_imageService.DeleteImage(productViewModel.ImageURL);
				string? newImageURL = _imageService.UploadImage(productViewModel.ImageFile);
				productViewModel.ImageURL = newImageURL;
			}

			Product product = _mapper.Map<Product>(productViewModel);
			_uow.Products.Update(product);
			await _uow.SaveChangesAsync();
		}

		public async Task<bool> DeleteProductAsync(int id)
		{
			ProductResponse? productResponse = await GetProductById(id);
			if (productResponse == null)
				return false;

			_imageService.DeleteImage(productResponse.ImageURL);
			var productEntity = await _uow.Products.GetByIdAsync(id, x => x.Category);
			if (productEntity != null)
			{
				_uow.Products.Delete(productEntity);
				await _uow.SaveChangesAsync();
				return true;
			}
			return false;
		}
	}
}
