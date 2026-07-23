using AutoMapper;
using myshop.Models.DTOs;
using myshop.Models.Entities;
using myshop.Models.ViewModels;

namespace myshop.Web.Mappings
{
	public class CategoryProfile : Profile
	{
		public CategoryProfile()
		{
			CreateMap<CategoryViewModel, Category>();
			CreateMap<Category, CategoryResponse>();
			CreateMap<CategoryResponse, CategoryViewModel>();
		}
	}
}