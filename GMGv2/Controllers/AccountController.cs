using GMG.Application.Common;
using GMG.Application.Feactures.Account.Commands.CreateUser;
using GMG.Application.Feactures.Account.Queries.GetBranchUser;
using GMG.Application.Feactures.Account.Queries.GetUser;
using GMGv2.Models;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GMGv2.Controllers
{
    public class AccountController(IUserContext userContext, IMediator mediator) : Controller
    {
        [HttpGet, AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpGet, AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
                return View(model);

            var user = await mediator.Send(new GetUserQuery(model.Email, model.Password));

            if (user is not null)
            {
                var claims = new List<Claim>
                {
                    new (ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new (ClaimTypes.Email, user.Email),
                    new (ClaimTypes.Name, user.Username),
                    new ("UserType", "Owner"),
                    new ("FullName", $"{user.FirstName} {user.LastName}"),
                    new (ClaimTypes.Role, user.UserRole.ToString()),
                    new ("BranchId", user.BranchId),
                    new ("BranchName", user.BranchName)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = model.RememberMe,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                return RedirectToLocal(returnUrl);
            }

            // Try how branch user

            var branchUser = await mediator.Send(new GetBranchUserQuery(model.Email, model.Password));
            if (branchUser is not null)
            {
                var claims = new List<Claim>
                {
                    new (ClaimTypes.NameIdentifier, branchUser.Id.ToString()),
                    new (ClaimTypes.Email, branchUser.Email),
                    new (ClaimTypes.Name, branchUser.Username),
                    new ("UserType", "BranchUser"),
                    new ("BranchUserId", branchUser.Id),
                    new ("BranchId", branchUser.BranchId),
                    new ("FullName", $"{branchUser.FirstName} {branchUser.LastName}"),
                    new ("BranchName", branchUser.BranchName),
                    new (ClaimTypes.Role, branchUser.BranchUserRole.ToString())
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = model.RememberMe,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                return RedirectToLocal(returnUrl);
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(model);
        }

        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (model.Password != model.ConfirmPassword)
            {
                ModelState.AddModelError(string.Empty, "Passwords do not match.");
                return View(model);
            }

            var createResult = await mediator.Send(new CreateUserCommand(
                model.Username,
                model.Email,
                model.Password,
                model.FirstName,
                model.LastName));

            var user = createResult.Value;

            var claims = new List<Claim>
            {
                new (ClaimTypes.NameIdentifier, user.Id.ToString()),
                new (ClaimTypes.Email, user.Email),
                new (ClaimTypes.Name, user.Username),
                new ("UserType", "Owner"),
                new ("FullName", $"{user.FirstName} {user.LastName}"),
                new (ClaimTypes.Role, user.UserRole.ToString()),
                new ("BranchId", user.BranchId),
                new ("BranchName", user.BranchName)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));

            return RedirectToAction("Index", "Home");
        }

        private IActionResult RedirectToLocal(string returnUrl)
            => Url.IsLocalUrl(returnUrl) ? Redirect(returnUrl) : RedirectToAction("Index", "Home");
    }
}
