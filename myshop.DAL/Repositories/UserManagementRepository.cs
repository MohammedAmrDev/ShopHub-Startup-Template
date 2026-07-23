using Microsoft.EntityFrameworkCore;
using myshop.DAL.Data;
using myshop.DAL.Interfaces;
using myshop.Models.IdentityEntities;
using myshop.Models.ViewModels;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace myshop.DAL.Repositories
{
	public class UserManagementRepository : IUserManagementRepository
	{
		private readonly ApplicationDbContext _context;
		public UserManagementRepository(ApplicationDbContext context) =>
			_context = context;

		public async Task<List<UserManagementViewModel>> GetAllForDataTableAsync(Expression<Func<ApplicationUser, bool>>? filter, string? orderBy, string? orderDir, int start, int length)
		{
			var query = _context.Users.AsNoTracking().AsQueryable();

			if (filter is not null)
				query = query.Where(filter);

			if (orderBy is not null)
				query = query.OrderBy(orderBy + " " + (orderDir ?? "asc"));

			query = query.Skip(start).Take(length);

			var result = query.Select(u => new UserManagementViewModel
			{
				Id = u.Id.ToString(),
				Username = u.UserName!,
				Email = u.Email!,
				Role = _context.UserRoles
					.Where(ur => ur.UserId == u.Id)
					.Join(
						_context.Roles,
						ur => ur.RoleId,
						r => r.Id,
						(ur, r) => r.Name
					)
					.First()!,
				IsLocked = u.LockoutEnd != null && u.LockoutEnd > DateTimeOffset.UtcNow
			});

			return await result.ToListAsync();
		}

		public async Task<int> GetCountAsync() =>
			await _context.Users.CountAsync();
	}
}
