using Microsoft.Extensions.DependencyInjection;
using myshop.BLL.Interfaces;
using myshop.BLL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myshop.BLL
{
	public static class ConfiguringBusinessLogic
	{
		public static IServiceCollection ConfiguringBusinessLogicLayer(this IServiceCollection services)
		{
			services.AddScoped<IProductsService, ProductsService>();
			services.AddScoped<ICategoriesService, CategoriesService>();
			services.AddScoped<IFileService, LocalFileService>();
			services.AddScoped<IUserManagementService, UserManagementService>();
			services.AddTransient<IMailService, MailService>();
			return services;
		}
	}
}
