using AutoMapper;
using Microsoft.AspNetCore.Identity;
using myshop.Models.IdentityEntities;
using myshop.Models.ViewModels;
using System.Net.Mail;

namespace myshop.Web.Mappings
{
	public class ApplicationUserProfile : Profile
	{
		public ApplicationUserProfile()
		{
			CreateMap<RegistrationViewModel, ApplicationUser>().ForMember(u => u.UserName, opt => opt.MapFrom(src => new MailAddress(src.Email).User));
			CreateMap<ApplicationUser, UserManagementViewModel>().ReverseMap();
		}
	}
}
