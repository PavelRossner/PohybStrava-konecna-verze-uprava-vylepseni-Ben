using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PohybStrava.Data;
using PohybStrava.Models;

namespace PohybStrava.Controllers
{
    public class FoodDatabaseController : Controller
    {
        private readonly ApplicationDbContext db;

        public FoodDatabaseController(ApplicationDbContext context)
        {
            db = context;
        }

        // GET: FoodDatabase
        public async Task<IActionResult> Index()
        {
              return View(await db.FoodDatabase.ToListAsync());
        }

        // GET: FoodDatabase/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || db.FoodDatabase == null)
            {
                return NotFound();
            }

            var foodDatabase = await db.FoodDatabase
                .FirstOrDefaultAsync(m => m.FoodDatabaseId == id);
            if (foodDatabase == null)
            {
                return NotFound();
            }

            return View(foodDatabase);
        }

        // GET: FoodDatabase/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: FoodDatabase/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FoodDatabaseId,UserId,FoodItem,Unit,FoodDatabaseItem,Note")] FoodDatabase foodDatabase, User user)
        {
            if (ModelState.IsValid)
            {
                user = db.User.FirstOrDefault(u => u.Email == this.User.Identity.Name);
                foodDatabase.UserId = user.Id;

                db.Add(foodDatabase);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(foodDatabase);
        }

        // GET: FoodDatabase/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || db.FoodDatabase == null)
            {
                return NotFound();
            }

            var foodDatabase = await db.FoodDatabase.FindAsync(id);
            if (foodDatabase == null)
            {
                return NotFound();
            }
            return View(foodDatabase);
        }

        // POST: FoodDatabase/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FoodDatabaseId,UserId,FoodItem,Unit,FoodDatabaseItem,Note")] FoodDatabase foodDatabase)
        {
            if (id != foodDatabase.FoodDatabaseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(foodDatabase);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FoodDatabaseExists(foodDatabase.FoodDatabaseId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(foodDatabase);
        }

        // GET: FoodDatabase/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || db.FoodDatabase == null)
            {
                return NotFound();
            }

            var foodDatabase = await db.FoodDatabase
                .FirstOrDefaultAsync(m => m.FoodDatabaseId == id);
            if (foodDatabase == null)
            {
                return NotFound();
            }

            return View(foodDatabase);
        }

        // POST: FoodDatabase/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (db.FoodDatabase == null)
            {
                return Problem("Entity set 'ApplicationDbContext.FoodDatabase'  is null.");
            }
            var foodDatabase = await db.FoodDatabase.FindAsync(id);
            if (foodDatabase != null)
            {
                db.FoodDatabase.Remove(foodDatabase);
            }
            
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FoodDatabaseExists(int id)
        {
          return db.FoodDatabase.Any(e => e.FoodDatabaseId == id);
        }
    }
}
