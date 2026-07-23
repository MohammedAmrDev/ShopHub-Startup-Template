using Microsoft.EntityFrameworkCore;
using myshop.DAL.Data;
using myshop.DAL.Interfaces;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace myshop.DAL.Repositories
{
	public class GenericRepository<T> : IGenericRepository<T> where T : class
	{
		private readonly ApplicationDbContext _context;
		private readonly DbSet<T> _dbSet;

		public GenericRepository(ApplicationDbContext context)
		{
			_context = context;
			_dbSet = _context.Set<T>();
		}
		public async Task<int> GetCountAsync() =>
			await _dbSet.CountAsync();

		public async Task<List<T>> GetAllAsync()
		{
			return await _dbSet.ToListAsync();
		}

		public async Task<List<T>> GetAllForDataTableAsync(Expression<Func<T, bool>>? filter, string? orderBy, string? orderDir, int start, int length, Expression<Func<T, object>>? include = null)
		{
			var query = _dbSet.AsQueryable();

			if (include is not null)
				query = query.Include(include);

			if (filter is not null)
				query = query.Where(filter);

			if (orderBy is not null)
				query = query.OrderBy(orderBy + " " + (orderDir ?? "asc"));

			query = query.Skip(start).Take(length);

			return await query.ToListAsync();
		}

		public async Task<T?> GetByIdAsync(int id)
		{
			return await _dbSet.FindAsync(id);
		}

		public async Task CreateAsync(T entity)
		{
			await _dbSet.AddAsync(entity);
		}
		public void Update(T newEntity)
		{
			_dbSet.Update(newEntity);
		}
		public void Delete(T entity)
		{
			_dbSet.Remove(entity);
		}
	}
}
