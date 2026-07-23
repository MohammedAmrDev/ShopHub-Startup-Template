using AutoMapper;
using myshop.BLL.Interfaces;
using myshop.DAL.Interfaces;
using myshop.Models.DTOs;
using myshop.Models.Entities;
using myshop.Models.ViewModels;

namespace myshop.BLL.Services
{
	public class CategoriesService : ICategoriesService
	{
		private readonly IUnitOfWork _uow;
		private readonly IMapper _mapper;
		public CategoriesService(IUnitOfWork uow, IMapper mapper)
		{
			_uow = uow;
			_mapper = mapper;
		}

		public async Task<(List<CategoryResponse> data, int recordsTotal, int recordsFiltered)> GetCategoriesAsync(string? search, string? orderBy, string? orderDir, int? start, int? length)
		{
			var categories = await _uow.Categories.GetAllForDataTableAsync(
				string.IsNullOrEmpty(search) ? null : p => p.Name.ToLower().Contains(search.ToLower()) || p.Description.ToLower().Contains(search.ToLower()),
				orderBy,
				orderDir,
				start ?? 0,
				length ?? 5
			);

			var data = categories.Select(_mapper.Map<CategoryResponse>).ToList();

			return (data, await _uow.Categories.GetCountAsync(), data.Count());
		}

		public async Task<List<CategoryResponse>> GetCategoriesAsync()
		{
			IEnumerable<Category> categories = await _uow.Categories.GetAllAsync();
			return categories.Select(_mapper.Map<CategoryResponse>).ToList();
		}

		public async Task<CategoryResponse?> GetCategoryByIdAsync(int id)
		{
			var category = await _uow.Categories.GetByIdAsync(id);
			return _mapper.Map<CategoryResponse>(category);
		}

		public async Task CreateCategoryAsync(CategoryViewModel category)
		{
			await _uow.Categories.CreateAsync(_mapper.Map<Category>(category));
			await _uow.SaveChangesAsync();
		}

		public async Task UpdateCategoryAsync(CategoryViewModel categoryVM)
		{
			_uow.Categories.Update(_mapper.Map<Category>(categoryVM));
			await _uow.SaveChangesAsync();
		}

		public async Task<bool> DeleteCategoryAsync(int id)
		{
			var category = await _uow.Categories.GetByIdAsync(id);
			if (category is null)
				return false;

			_uow.Categories.Delete(category);
			await _uow.SaveChangesAsync();
			return true;
		}
	}
}
