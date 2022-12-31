using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PohybStrava.Data;
using PohybStrava.Models;
using PohybStrava.Models.Response;
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
            //List<UserResponse> users = db.User
            //     .Where(u => u.Email == this.User.Identity.Name || User.Identity.Name.Contains("admin"))
            //     .Select(UserResponse.GetUserResponse)
            //     .ToList();

            return RedirectToAction("Index", "Users");
        }


        // GET: Account/Edit/5
        public IActionResult Edit(string id)
        {
            if (id == null || this.db.User == null)
            {
                return NotFound();
            }

            User user = this.db.User.FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Account/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, [Bind("FirstName,LastName,DateOfBirth,Email,Gender")] User user)
        {
            if (id != user.Id)

            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(user);
            }

            User dbUser = this.db.User.FirstOrDefault(u => u.Id == id);

            if (dbUser == null)
            {
                return View(user);
            }

            dbUser.FirstName = user.FirstName;
            dbUser.LastName = user.LastName;
            dbUser.Email = user.Email;
            dbUser.DateOfBirth = user.DateOfBirth;
            dbUser.Gender = user.Gender;

            this.db.SaveChanges();
            return RedirectToAction(nameof(Overview));
        }


        // GET: Account/Delete/5
        public IActionResult Delete(string id)
        {
            if (id == null || db.User == null)
            {
                return NotFound();
            }

            User user = db.User.FirstOrDefault(u => u.Id == id);

            return View(user);
        }

        // POST: Account/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(string id)
        {
            User user = db.User.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                db.User.Remove(user);
            }

            db.SaveChanges();
            return RedirectToAction(nameof(Overview));
        }

    }
}



