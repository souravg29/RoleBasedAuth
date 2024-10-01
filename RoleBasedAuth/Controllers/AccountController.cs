using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RoleBasedAuth.Models.Domain;
using RoleBasedAuth.Models.ViewModel;

namespace RoleBasedAuth.Controllers
{
	public class AccountController : Controller
	{
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<ApplicationRole> _roleManager;

		public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
		{
			this._signInManager = signInManager;
			this._userManager = userManager;
			this._roleManager = roleManager;
		}

		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel loginViewModel)
		{
			if (!ModelState.IsValid)
			{
				return View(loginViewModel);
			}
			Console.WriteLine($"loginViewModel: {loginViewModel.Email}");

			var appUser = await _userManager.FindByEmailAsync(loginViewModel.Email);

			if (appUser != null)
			{

				var result = await _signInManager.PasswordSignInAsync(appUser, loginViewModel.Password, false, false);

				if (result.Succeeded)
				{
					return RedirectToAction("Index", "Home");
				}


			}

			return View();
		}


		[Authorize(Roles = "SuperAdmin")]
		public IActionResult Register()
		{
			return View();
		}

		[Authorize(Roles = "SuperAdmin")]
		[HttpPost]
		public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
		{
			if (!ModelState.IsValid)
			{
				return View(registerViewModel);
			}

			var appUser = new ApplicationUser()
			{
				Name = registerViewModel.Email.Substring(0, registerViewModel.Email.IndexOf("@")),
				UserName = registerViewModel.Email,
				Email = registerViewModel.Email
			};

			var result = await _userManager.CreateAsync(appUser, registerViewModel.Password);

			if (result.Succeeded)
			{
				string role = registerViewModel.Role;

				if (!await _roleManager.RoleExistsAsync(role))
				{
					ModelState.AddModelError("", $"Role '{role}' does not exist.");
					return View(registerViewModel);
				}


				await _userManager.AddToRoleAsync(appUser, role);

				return RedirectToAction("Index", "Home");


			}

			return View();
		}


		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction("Login");
		}

	}
}
