using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using myshop.BLL.Interfaces;
using myshop.Models.Enums;
using myshop.Models.IdentityEntities;

namespace myshop.Web.Controllers
{
	[Authorize(Roles = "Admin")]
	public class UserManagementController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IUserManagementService _userManagementService;
		public UserManagementController(UserManager<ApplicationUser> userManager, IUserManagementService userManagementService)
		{
			_userManager = userManager;
			_userManagementService = userManagementService;
		}
		public async Task<IActionResult> Index()
		{
			return View();
		}

		public async Task<IActionResult> GetData()
		{
			int? length = int.TryParse(Request.Form["length"][0], out int _length) ? _length : null;
			int? start = int.TryParse(Request.Form["start"][0], out int _start) ? _start : null;
			var search = Request.Form["search[value]"][0];
			var orderByColumnName = Request.Form[$"columns[{Request.Form["order[0][column]"][0]}][name]"][0];
			var orderDir = Request.Form["order[0][dir]"][0];

			var (data, recordsTotal, recordsFiltered) = await _userManagementService.GetUsersAsync(search, orderByColumnName, orderDir, start, length);

			return Json(new
			{
				draw = Request.Form["draw"][0],
				data,
				recordsTotal,
				recordsFiltered,
			});
		}

		[HttpPost]
		public async Task<IActionResult> ToggleLock(string id)
		{
			var user = await _userManager.FindByIdAsync(id);

			if (user is null)
				return BadRequest("Invalid User Id");

			if (AdminGuarding(id)) return BadRequest();

			if (await _userManager.IsLockedOutAsync(user))
			{
				await _userManager.SetLockoutEndDateAsync(user, null);
				return Json(new { success = true, message = "User is activiated" });
			}
			else
			{
				await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddYears(100));
				return Json(new { success = true, message = "User is locked" });
			}

		}

		[HttpPost]
		public async Task<IActionResult> ToggleRole(string id)
		{
			var user = await _userManager.FindByIdAsync(id);
			if (user is null)
				return BadRequest("Invalid User Id");
			if (AdminGuarding(id)) return BadRequest();

			var isCustomer = await _userManager.IsInRoleAsync(user, nameof(UserTypeEnum.Customer));
			if (isCustomer)
			{
				await _userManager.AddToRoleAsync(user, nameof(UserTypeEnum.Admin));
				await _userManager.RemoveFromRoleAsync(user, nameof(UserTypeEnum.Customer));
				return Json(new { success = true, message = "Role Changed To Admin" });
			}
			else
			{
				await _userManager.AddToRoleAsync(user, nameof(UserTypeEnum.Customer));
				await _userManager.RemoveFromRoleAsync(user, nameof(UserTypeEnum.Admin));
				return Json(new { success = true, message = "Role Changed To Customer" });
			}
		}

		[HttpDelete]
		public async Task<IActionResult> Delete(string id)
		{
			var user = await _userManager.FindByIdAsync(id);
			if (user is null)
				return BadRequest("Invalid User Id");

			if (AdminGuarding(id))
				return BadRequest();

			var result = await _userManager.DeleteAsync(user);

			if (result.Succeeded)
				return Json(new { success = true, message = "User has been deleted" });

			return Json(new { success = false, message = "Error occured while deleted" });
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