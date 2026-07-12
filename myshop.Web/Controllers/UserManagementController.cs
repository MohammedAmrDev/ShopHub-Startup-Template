using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using myshop.Models.Enums;
using myshop.Models.IdentityEntities;
using myshop.Models.ViewModels;

namespace myshop.Web.Controllers
{
	public class UserManagementController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IMapper _mapper;
		public UserManagementController(UserManager<ApplicationUser> userManager, IMapper mapper)
		{
			_userManager = userManager;
			_mapper = mapper;
		}
		public async Task<IActionResult> Index(string? searchBy, int? pageIndex = 0)
		{
			var pageSize = 5;
			var usersFiltered = _userManager.Users
				.Where(u => searchBy == null || u.UserName.Contains(searchBy));

			var usersPaginated = await usersFiltered.Skip(pageSize * pageIndex.Value).Take(pageSize).ToListAsync();
			
			var usersVM = new List<UserManagementViewModel>();
			
			foreach (var user in usersPaginated)
			{
				var vm = _mapper.Map<UserManagementViewModel>(user);
				var userRoles = await _userManager.GetRolesAsync(user);
				vm.Role = userRoles.FirstOrDefault() ?? "Customer";
				vm.IsLocked = await _userManager.IsLockedOutAsync(user);
				usersVM.Add(vm);
			}

			ViewBag.PagesNum = (int)Math.Ceiling(usersFiltered.Count() / (double)pageSize);
			ViewBag.PageIndex = pageIndex;
			ViewBag.SearchBy = searchBy;

			return View(usersVM);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> ToggleLock(string id)
		{
			var user = await _userManager.FindByIdAsync(id);
			if (user == null) return NotFound();
			if(AdminGuarding(id)) return BadRequest();

			if (await _userManager.IsLockedOutAsync(user))
			{
				await _userManager.SetLockoutEndDateAsync(user, null);
			}
			else
			{
				await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddYears(100));
			}

			return RedirectToAction(nameof(Index));
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> ToggleRole(string id)
		{
			var user = await _userManager.FindByIdAsync(id);
			if (user == null) return NotFound();
			if(AdminGuarding(id)) return BadRequest();

			var isCustomer = await _userManager.IsInRoleAsync(user, nameof(UserTypeEnum.Customer));
			if (isCustomer)
			{
				await _userManager.AddToRoleAsync(user, nameof(UserTypeEnum.Admin));
				await _userManager.RemoveFromRoleAsync(user, nameof(UserTypeEnum.Customer));
			}
			else
			{
				await _userManager.AddToRoleAsync(user, nameof(UserTypeEnum.Customer));
				await _userManager.RemoveFromRoleAsync(user, nameof(UserTypeEnum.Admin));
			}

			return RedirectToAction(nameof(Index));
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete(string id)
		{
			var user = await _userManager.FindByIdAsync(id);
			if (user == null) return NotFound();
			if (AdminGuarding(id)) return BadRequest();

			var result = await _userManager.DeleteAsync(user);

			if (result.Succeeded)
				return RedirectToAction(nameof(Index));

			return BadRequest();
		}

		// Must be added after checking if there is a user with the specified id
		// Can be extended in the future
		private bool AdminGuarding(string id)
		{
			var currentUserId =  _userManager.GetUserId(User);
			if (currentUserId == id) return true;
			return false;
		}
	}
}