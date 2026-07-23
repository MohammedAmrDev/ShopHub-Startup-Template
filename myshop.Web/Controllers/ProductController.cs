using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using myshop.BLL.Interfaces;
using myshop.Models.DTOs;
using myshop.Models.ViewModels;

namespace myshop.Web.Areas.Admin.Controllers
{
	[Authorize(Roles = "Admin")]
	public class ProductController : Controller
    {
        private readonly IProductsService _productsService;
        private readonly ICategoriesService _categoryService;
        private readonly IMapper _mapper;

		public ProductController(IProductsService productsService, ICategoriesService categoryService, IMapper mapper)
        {
            _productsService = productsService;
            _categoryService = categoryService;
            _mapper = mapper;
        }

        public IActionResult Index()
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


            var (data, recordsTotal, recordsFiltered) = await _productsService.GetAllProducts(search, orderByColumnName, orderDir, start, length);

            return Json(new
            {
                draw = Request.Form["draw"][0],
				recordsTotal,
				recordsFiltered,
				data,
            });
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            List<CategoryResponse> categories = await _categoryService.GetCategoriesAsync();

			ProductViewModel productViewModel = new ProductViewModel
            { 
                CategoryList = categories.Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() })
            };

            return View(productViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductViewModel productViewModel)
        {
            if (ModelState.IsValid)
            {
                await _productsService.CreateProduct(productViewModel);
                TempData["Create"] = "Item has Created Successfully";
                return RedirectToAction("Index");
            }

			List<CategoryResponse> categories = await _categoryService.GetCategoriesAsync();
			productViewModel.CategoryList = categories.Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() });

			return View(productViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
			if (id is null)
				return BadRequest("Invalid Category Id");

			ProductResponse? productResponse = await _productsService.GetProductById(id.Value);

			if (productResponse is null)
				return NotFound();

			ProductViewModel productViewModel = _mapper.Map<ProductViewModel>(productResponse);
			List<CategoryResponse> categories = await _categoryService.GetCategoriesAsync();
			productViewModel.CategoryList = categories.Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() });

			return View(productViewModel);
        }
        
        [HttpPost]
        public async Task<IActionResult> Edit(ProductViewModel productViewModel)
        {
            if (ModelState.IsValid)
            {
                await _productsService.UpdateProduct(productViewModel);
                TempData["Update"] = "Data has Updated Successfully";
                return RedirectToAction("Index");
            }

			List<CategoryResponse> categories = await _categoryService.GetCategoriesAsync();
			productViewModel.CategoryList = categories.Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() });

			return View(productViewModel);
        }
        
        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null)
				return BadRequest("Invalid Category Id");

			bool isDeleted = await _productsService.DeleteProductAsync(id.Value);

            if (isDeleted)
			    return Json(new { success = true, message = "file has been Deleted" });

            return Json(new { success = false, message = "Error while Deleting" });
        }
    }
}
