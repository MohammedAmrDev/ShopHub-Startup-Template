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
			CreateMap<Product, ProductResponse>().ForMember(pr => pr.ImageURL, opt => opt.MapFrom(src => src.Img));
			CreateMap<ProductViewModel, Product>().ForMember(p => p.Img, opt => opt.MapFrom(src => src.ImageURL));
			CreateMap<ProductResponse, ProductViewModel>();
		}
	}
}
