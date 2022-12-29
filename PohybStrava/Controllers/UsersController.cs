using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PohybStrava.Data;
using PohybStrava.Models;

namespace PohybStrava.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext db;

        public UsersController(ApplicationDbContext db)
        {
           this.db = db;
        }


        // GET: Users
        public IActionResult Index()
        {
            var Id = User.Identity.GetUserId();
            User user = this.db.User.FirstOrDefault(u => u.Id == Id);

            if (user == null)
            {
                return RedirectToAction("Error", "Users");
            }

            if (User.Identity.Name.Contains("admin"))

            {
                return View(this.db.User.ToList());
            }

            else

            {
                return View((this.db.User.Where(u => u.Id == Id)).ToList());
            }

        }


        // GET: Users/Details/5
        [HttpGet("Details")]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || this.db.User == null)
            {
                return NotFound();
            }

            var user = await this.db.User
                .FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("FirstName,LastName,DateOfBirth,Email,Gender")] User user)
        {
            user = this.db.User.FirstOrDefault(u => u.Email == this.User.Identity.Name);

            if (ModelState.IsValid)
            {
                this.db.Add(user);
                this.db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Edit/5
        public IActionResult Edit(string id)
        {
            string name = this.User.Identity.Name;

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

        // POST: Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, [Bind("FirstName,LastName,DateOfBirth,Email,Gender")] User user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var dbUser = this.db.User.FirstOrDefault(u => u.Id == id);

                    if (user.FirstName != null && user.FirstName != "")
                        dbUser.FirstName = user.FirstName;

                    if (user.LastName != null && user.LastName != "")
                        dbUser.LastName = user.LastName;

                    if (user.Email != null && user.Email != "")
                        dbUser.Email = user.Email;

                    if (user.Gender != null && user.Gender != "")
                        dbUser.Gender = user.Gender;

                    this.db.SaveChanges();

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public IActionResult Delete(string id)
        {
            if (id == null || this.db.User == null)
            {
                return NotFound();
            }

            var user = this.db.User
                .FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(string id)
        {
            if (this.db.User == null)
            {
                return Problem("Entity set 'ApplicationDbContext.User'  is null.");
            }
            var user = this.db.User.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                this.db.User.Remove(user);
            }

            this.db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(string id)
        {
            return this.db.User.Any(u => u.Id == id);
        }


        // GET: Users/Error
        public IActionResult Error()
        {
            return View();
        }
    }
}
