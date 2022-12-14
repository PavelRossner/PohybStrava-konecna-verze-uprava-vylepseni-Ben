using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PohybStrava.Data;
using PohybStrava.Models;

namespace PohybStrava.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext db;

        public UsersController(ApplicationDbContext context)
        {
            db = context;
        }


        // GET: Users
        public IActionResult Index()
        {
            bool user = db.User.Any(u => u.Email == this.User.Identity.Name);
            if (user == false)
            {
                return RedirectToAction("Error", "Users");
            }

            var users = db.User.Where(u => u.Email == this.User.Identity.Name || User.Identity.Name.Contains("admin"));

            return View(users);
        }


        // GET: Users/Details/5
        [HttpGet("Details")]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || db.User == null)
            {
                return NotFound();
            }

            var user = await db.User
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
            user = db.User.FirstOrDefault(u => u.Email == this.User.Identity.Name);

            if (ModelState.IsValid)
            {
                db.Add(user);
                db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Edit/5
        public IActionResult Edit(string id)
        {
            var name = this.User.Identity.Name;

            if (id == null || db.User == null)
            {
                return NotFound();
            }

            var user = db.User.FirstOrDefault(u => u.Id == id);
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
                    var dbUser = db.User.FirstOrDefault(u => u.Id == id);

                    if (user.FirstName != null && user.FirstName != "")
                        dbUser.FirstName = user.FirstName;

                    if (user.LastName != null && user.LastName != "")
                        dbUser.LastName = user.LastName;

                    if (user.Email != null && user.Email != "")
                        dbUser.Email = user.Email;

                    if (user.Gender != null && user.Gender != "")
                        dbUser.Gender = user.Gender;

                    db.SaveChanges();

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
            if (id == null || db.User == null)
            {
                return NotFound();
            }

            var user = db.User
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
            if (db.User == null)
            {
                return Problem("Entity set 'ApplicationDbContext.User'  is null.");
            }
            var user = db.User.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                db.User.Remove(user);
            }

            db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(string id)
        {
            return db.User.Any(u => u.Id == id);
        }


        // GET: Users/Error
        public IActionResult Error()
        {
            return View();
        }
    }
}
