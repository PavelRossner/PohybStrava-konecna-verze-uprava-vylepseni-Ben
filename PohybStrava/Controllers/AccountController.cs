using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PohybStrava.Data;
using PohybStrava.Models;
using System.Security.Claims;
using System.Security.Policy;

namespace PohybStrava.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly ApplicationDbContext db;

        public AccountController
        (
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ApplicationDbContext db
        )
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.db = db;
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }


        [HttpGet]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    DateOfBirth = model.DateUser,
                    Gender = model.Gender,
                };

                db.SaveChanges();

                var result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, isPersistent: false);

                    var modeluser = new User
                    {
                        Email = model.Email,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                    };

                    return RedirectToLocal(returnUrl);

                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }


        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnURL = null)
        {
            ViewData["ReturnUrl"] = returnURL;
            if (ModelState.IsValid)
            {
                Microsoft.AspNetCore.Identity.SignInResult resultValidation = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (resultValidation.Succeeded)
                {
                    return RedirectToLocal(returnURL);
                }


                else
                {
                    ModelState.AddModelError(string.Empty, "Uživatel není registrován, nebo je zadáno nesprávné heslo.");
                    return View(model);
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }


        public IActionResult Administration()
        {
            return View();
        }

        public IActionResult Overview()
        {
            var users = db.User.Where(u => u.Email == this.User.Identity.Name || User.Identity.Name.Contains("admin"));

            return View(users);
        }

        public IActionResult UsersDatabase()
        {
            return View(db.Users.ToList());
        }


        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            var user = await db.User.FirstOrDefaultAsync(m => m.Id == id);

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await db.User.FirstOrDefaultAsync(m => m.Id == id);
            if (user != null)
            {
                db.User.Remove(user);
            }

            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Overview));
        }

    }
}


