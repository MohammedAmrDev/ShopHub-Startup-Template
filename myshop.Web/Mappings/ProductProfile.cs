using AutoMapper;
using myshop.Models.DTOs;
using myshop.Models.Entities;
using myshop.Models.ViewModels;

namespace myshop.Web.Mappings
{
	public class ProductProfile : Profile
	{
		public ProductProfile()
		{
			CreateMap<Product, ProductResponse>();
			CreateMap<ProductViewModel, Product>();
			CreateMap<ProductResponse, ProductViewModel>();
		}
	}
}
