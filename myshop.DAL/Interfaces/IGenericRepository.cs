using System.Linq.Expressions;

namespace myshop.DAL.Interfaces
{
	public interface IGenericRepository<T>
	{
		Task<int> GetCountAsync();
		Task<List<T>> GetAllAsync();
		Task<List<T>> GetAllForDataTableAsync(Expression<Func<T, bool>>? filter, string? orderBy, string? orderDir, int start, int length, Expression<Func<T, object>>? include = null);
		Task<T?> GetByIdAsync(int id);
		Task CreateAsync(T entity);
		void Update(T newEntity);
		void Delete(T entity);
	}
}