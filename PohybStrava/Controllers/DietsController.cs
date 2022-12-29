using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PohybStrava.Data;
using PohybStrava.Models;
using PohybStrava.Models.Response;

namespace PohybStrava.Controllers
{
    public class DietsController : Controller
    {
        private readonly ApplicationDbContext db;

        public DietsController(ApplicationDbContext db)
        {
            this.db = db;
        }

        // GET: Diets
        public async Task<IActionResult> Index()
        {
            string Id = User.Identity.GetUserId();
            User user = await db.User.FirstOrDefaultAsync(u => u.Id == Id);

            if (user == null)
            {
                return RedirectToAction("Error", "Diets");
            }

            if (User.Identity.Name.Contains("admin"))
            {
                return View(db.Diet.OrderBy(d => d.DateDiet)
                                   .Select(DietResponse.GetDietResponse)
                                   .ToList());
            }
            else
            {
                return View(db.Diet.OrderBy(d => d.DateDiet)
                                     .Where(u => u.UserId == Id)
                                     .Select(DietResponse.GetDietResponse)
                                     .ToList());
            }
        }

        // GET: Diets/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null || db.Diet == null)
            {
                return NotFound();
            }

            var diet = db.Diet.Select(DietResponse.GetDietResponse)
                               .FirstOrDefault(d => d.DietId == id);

            if (diet == null)
            {
                return NotFound();
            }

            return View(diet);
        }


        // GET: Diets/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Diets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DietId,UserId,Email,DateDiet,Food,EnergyDiet,Amount,EnergyDietSum,Day,Month,Year,EnergyDietTotal")] Diet diet, User user)
        {
            if (ModelState.IsValid)
            {
                user = db.User.FirstOrDefault(u => u.Id == this.User.Identity.GetUserId());

                user.Diet.Add(diet);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Diets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || db.Diet == null)
            {
                return NotFound();
            }

            Diet diet = await db.Diet.FindAsync(id);

            if (diet == null)
            {
                return NotFound();
            }
            return View(diet);
        }

        // POST: Diets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Diet diet)
        {
            if (id != diet.DietId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(diet);
            }

            Diet dbDiet = db.Diet.FirstOrDefault(d => d.DietId == id);  //trackované property

            if (dbDiet == null)
            {
                return View(diet);
            }

            dbDiet.DateDiet = diet.DateDiet;
            dbDiet.Food = diet.Food;
            dbDiet.EnergyDiet = diet.EnergyDiet;
            dbDiet.Amount = diet.Amount;

            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Diets/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null || db.Diet == null)
            {
                return NotFound();
            }

            DietResponse dietResponse = db.Diet.Select(DietResponse.GetDietResponse)
                              .FirstOrDefault(d => d.DietId == id);
            if (dietResponse == null)
            {
                return NotFound();
            }

            return View(dietResponse);
        }

        // POST: Diets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (db.Diet == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Diet'  is null.");
            }
            Diet diet = await db.Diet.FindAsync(id);
            if (diet != null)
            {
                db.Diet.Remove(diet);
            }

            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DietExists(int id)
        {
            return db.Diet.Any(e => e.DietId == id);
        }



        // Diets - Day Overview
        public IActionResult SumDietsDay(int? id)
        {
            bool user = db.Users.Any(u => u.Email == this.User.Identity.Name);
            if (user == false)
            {
                return RedirectToAction("Error", "Diets");
            }

            IEnumerable<DietResponse> dietResponse = db.Diet.Select(DietResponse.GetDietResponse);

            IEnumerable<DietResponse> result =
             from s in dietResponse
             group s by new { date = new DateTime(s.DateDiet.Year, s.DateDiet.Month, s.DateDiet.Day) } into g
             select new Models.Response.DietResponse
             {
                 DateDiet = g.Key.date,
                 EnergyDietSum = (int)g.Sum(z => z.EnergyDietFoodTotal)
             };

            return View(result);
        }


        // Diets - Month Overview
        public IActionResult MonthDietsOverview(int? id)
        {
            bool user = db.Users.Any(u => u.Email == this.User.Identity.Name);
            if (user == false)
            {
                return RedirectToAction("Error", "Diets");
            }

            IEnumerable<DietResponse> dietResponse = db.Diet.Select(DietResponse.GetDietResponse);

            IEnumerable<DietResponse> result =
                from s in dietResponse
                group s by new { date = new DateTime(s.DateDiet.Year, s.DateDiet.Month, 1) } into g
                select new Models.Response.DietResponse
                {
                    DateDiet = g.Key.date,
                    EnergyDietSum = (int)g.Sum(z => z.EnergyDietFoodTotal)
                };

            return View(result);
        }


        // GET: Diets/Error
        public IActionResult Error()
        {
            return View();
        }


    }
}
