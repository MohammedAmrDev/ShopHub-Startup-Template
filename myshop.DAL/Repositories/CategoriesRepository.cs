using myshop.DAL.Data;
using myshop.DAL.Interfaces;
using myshop.Models.Entities;

namespace myshop.DAL.Repositories
{
	internal class CategoriesRepository : GenericRepository<Category>, ICategoriesRepository
	{
		public CategoriesRepository(ApplicationDbContext context) : base(context) { }
	}
}
