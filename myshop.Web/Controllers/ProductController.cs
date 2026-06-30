using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using myshop.BLL.Interfaces;
using myshop.Models.DTOs;
using myshop.Models.Entities;
using myshop.Models.ViewModels;

namespace myshop.Web.Areas.Admin.Controllers
{
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

        [HttpGet]
        public async Task<IActionResult> GetData()
        {
            var products = await _productsService.GetAllProducts();
            return Json(new { data = products });
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            List<Category> categories = await _categoryService.GetCategoriesAsync();

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

			List<Category> categories = await _categoryService.GetCategoriesAsync();
			productViewModel.CategoryList = categories.Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() });

			return View(productViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id == 0)
                return NotFound();

            ProductResponse? productResponse = await _productsService.GetProductById(id.Value);
            ProductViewModel productViewModel = _mapper.Map<ProductViewModel>(productResponse);
			List<Category> categories = await _categoryService.GetCategoriesAsync();
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

			List<Category> categories = await _categoryService.GetCategoriesAsync();
			productViewModel.CategoryList = categories.Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() });

			return View(productViewModel);
        }
        
        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            bool isDeleted = await _productsService.DeleteProductAsync(id.Value);

            if (isDeleted)
			    return Json(new { success = true, message = "file has been Deleted" });

			return Json(new { success = false, message = "Error while Deleting" });
        }
    }
}
