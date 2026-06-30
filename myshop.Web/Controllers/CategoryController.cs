using Microsoft.AspNetCore.Mvc;
using myshop.BLL.Interfaces;
using myshop.Models.Entities;

namespace myshop.Web.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoriesService _categoriesService;

        public CategoryController(ICategoriesService categoriesService)
        {
            _categoriesService = categoriesService;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _categoriesService.GetCategoriesAsync();
            return View(categories);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {
                await _categoriesService.CreateCategoryAsync(category);
                TempData["Create"] = "Item has Created Successfully";
                return RedirectToAction("Index");
            }
            return View(category);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var categoryIndb = await _categoriesService.GetCategoryByIdAsync(id.Value);

			if (categoryIndb == null)
				return NotFound();

			return View(categoryIndb);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                await _categoriesService.UpdateCategoryAsync(category);
                TempData["Update"] = "Data has Updated Successfully";
                return RedirectToAction("Index");
            }
            return View(category);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var categoryIndb = await _categoriesService.GetCategoryByIdAsync(id.Value);

			if (categoryIndb == null)
				return NotFound();

			return View(categoryIndb);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCategory(int? id)
        {
			if (id == null || id == 0)
			{
				return NotFound();
			}
			var categoryIndb = _categoriesService.GetCategoryByIdAsync(id.Value);
			if (categoryIndb == null)
				return NotFound();


            await _categoriesService.DeleteCategoryAsync(id.Value);

            TempData["Delete"] = "Item has Deleted Successfully";
            return RedirectToAction("Index");
        }
    }
}
