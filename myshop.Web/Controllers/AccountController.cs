using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using myshop.Models.Enums;
using myshop.Models.IdentityEntities;
using myshop.Models.ViewModels;
using System.Text;

namespace myshop.Web.Controllers
{
	[AllowAnonymous]
	public class AccountController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly IMapper _mapper;
		//private readonly IEmailSender _emailSender;
		public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IMapper mapper)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_mapper = mapper;
			//_emailSender = emailSender;
		}

		[HttpGet]
		public IActionResult Register(UserTypeEnum? role)
		{
			RegistrationViewModel model = new RegistrationViewModel
			{
				Role = role ?? UserTypeEnum.Customer,
			};
			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Register(RegistrationViewModel registrationViewModel)
		{
			if (!ModelState.IsValid)
				return View(registrationViewModel);

			ApplicationUser mappedUser = _mapper.Map<ApplicationUser>(registrationViewModel);
			IdentityResult result = await _userManager.CreateAsync(mappedUser, registrationViewModel.Password);
			if (result.Succeeded)
			{
				await _userManager.AddToRoleAsync(mappedUser, registrationViewModel.Role.ToString());

				//var user = await _userManager.FindByEmailAsync(mappedUser.Email);
				//var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
				//code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
				//var callbackUrl = Url.Page(
				//	"Account/ConfirmEmail",
				//	""
				//);


				return RedirectToAction("Login");
			}
			else
			{
				foreach (var error in result.Errors)
					ModelState.AddModelError(string.Empty, error.Description);
				return View(registrationViewModel);
			}
		}

		[HttpGet]
		public async Task<IActionResult> Login()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel loginViewModel, string? returnUrl)
		{
			ViewData["ReturnUrl"] = returnUrl;

			if (!ModelState.IsValid)
				return View(loginViewModel);

			ApplicationUser? user = await _userManager.FindByEmailAsync(loginViewModel.Email);

			if (user == null)
			{
				ModelState.AddModelError(string.Empty, "User not found");
				return View(loginViewModel);
			}

			var result = await _signInManager.PasswordSignInAsync(
				user,
				loginViewModel.Password,
				loginViewModel.RememberMe,
				lockoutOnFailure: true
			);

			if (result.Succeeded)
				return returnUrl == null ? RedirectToAction("Index", "Home") : LocalRedirect(returnUrl);

			ModelState.AddModelError(string.Empty, "Invalid login attempt");
			return View(loginViewModel);
		}

		[HttpPost]
		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction("Index", "Home");
		}

		//[HttpGet]
		//public async Task<IActionResult> RegisterConfirmation()
		//{

		//}
	}
}
