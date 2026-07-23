using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using myshop.BLL.Interfaces;
using myshop.Models.ViewModels;

namespace myshop.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
	public class CategoryController : Controller
    {
        private readonly ICategoriesService _categoriesService;
        private readonly IMapper _mapper;

        public CategoryController(ICategoriesService categoriesService, IMapper mapper)
        {
            _categoriesService = categoriesService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
			return View();
		}

        [HttpPost]
        public async Task<IActionResult> GetData()
        {
			int? length = int.TryParse(Request.Form["length"][0], out int _length) ? _length : null;
			int? start = int.TryParse(Request.Form["start"][0], out int _start) ? _start : null;
			var search = Request.Form["search[value]"][0];
			var orderByColumnName = Request.Form[$"columns[{Request.Form["order[0][column]"][0]}][name]"][0];
			var orderDir = Request.Form["order[0][dir]"][0];

			var (data, recordsTotal, recordsFiltered) = await _categoriesService.GetCategoriesAsync(search, orderByColumnName, orderDir, start, length);

			return Json(new
			{
				draw = Request.Form["draw"][0],
				recordsTotal,
				recordsFiltered,
				data,
			});
		}

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryViewModel category)
        {
            if (ModelState.IsValid)
            {
                await _categoriesService.CreateCategoryAsync(category);
                TempData["Create"] = "Item has Created Successfully";
                return RedirectToAction(nameof(Index));
			}
			return View(category);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null)
                return BadRequest("Invalid Category Id");

            var categoryIndb = await _categoriesService.GetCategoryByIdAsync(id.Value);

			if (categoryIndb is null)
				return NotFound();

			var categoryVM = _mapper.Map<CategoryViewModel>(categoryIndb);
			return View(categoryVM);
		}

        [HttpPost]
        public async Task<IActionResult> Edit(CategoryViewModel category)
        {
            if (ModelState.IsValid)
            {
                await _categoriesService.UpdateCategoryAsync(category);
                TempData["Update"] = "Data has Updated Successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

		[HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {
			if (id is null)
				return BadRequest("Invalid Category Id");

			var isDeleted = await _categoriesService.DeleteCategoryAsync(id.Value);

			if (isDeleted)
			    return Json(new { success = true, message = "Item has been Deleted" });

			return Json(new { success = false, message = "Error while Deleting" });
        }
    }
}
