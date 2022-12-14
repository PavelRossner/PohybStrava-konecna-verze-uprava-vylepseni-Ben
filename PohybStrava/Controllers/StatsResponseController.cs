using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PohybStrava.Data;
using PohybStrava.Models;
using PohybStrava.Models.Response;

namespace PohybStrava.Controllers
{
    public class StatsResponseController : Controller
    {
        private readonly ICollection<StatsResponse> StatsResponse;

        public StatsResponseController(StatsResponse data)
        {
            StatsResponse = (ICollection<StatsResponse>) data;
        }

        // GET: StatsResponse
        public async Task<IActionResult> Index()
        {

            bool user = db.User.Any(u => u.Email == this.User.Identity.Name);
            if (user == false)
            {
                return RedirectToAction("Error", "StatsResponse");
            }
            return View(await db.StatsResponse.OrderBy(d => d.UserDate).ToListAsync());

        }

        // GET: StatsResponse/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || db.StatsResponse == null)
            {
                return NotFound();
            }

            var statsResponse = await db.StatsResponse
                .FirstOrDefaultAsync(m => m.StatsId == id);
            if (statsResponse == null)
            {
                return NotFound();
            }

            return View(statsResponse);
        }

        // GET: StatsResponse/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: StatsResponse/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StatsId,UserId,UserDate,Email,Age,Weight,Height,Gender,BMI,BMR,Day,Month,Year,WeightAverage,BMIAverage")] StatsResponse statsResponse, User user)
        {
            if (ModelState.IsValid)
            {
                user = db.User.FirstOrDefault(u => u.Email == this.User.Identity.Name);
                statsResponse.UserId = user.Id;

                db.Add(statsResponse);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(statsResponse);
        }

        // GET: StatsResponse/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || db.StatsResponse == null)
            {
                return NotFound();
            }

            var statsResponse = await db.StatsResponse.FindAsync(id);
            if (statsResponse == null)
            {
                return NotFound();
            }
            return View(statsResponse);
        }

        // POST: StatsResponse/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StatsId,UserId,UserDate,Email,Age,Weight,Height,Gender,BMI,BMR,Day,Month,Year,WeightAverage,BMIAverage")] StatsResponse statsResponse)
        {
            if (id != statsResponse.StatsId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(statsResponse);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StatsResponseExists(statsResponse.StatsId))
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
            return View(statsResponse);
        }

        // GET: StatsResponse/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || db.StatsResponse == null)
            {
                return NotFound();
            }

            var statsResponse = await db.StatsResponse
                .FirstOrDefaultAsync(m => m.StatsId == id);
            if (statsResponse == null)
            {
                return NotFound();
            }

            return View(statsResponse);
        }

        // POST: StatsResponse/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (db.StatsResponse == null)
            {
                return Problem("Entity set 'ApplicationDbContext.StatsResponse'  is null.");
            }
            var statsResponse = await db.StatsResponse.FindAsync(id);
            if (statsResponse != null)
            {
                db.StatsResponse.Remove(statsResponse);
            }

            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StatsResponseExists(int id)
        {
            return db.StatsResponse.Any(e => e.StatsId == id);
        }




        // Users - Month Average
        public IActionResult AverageUser()
        {
            bool user = db.User.Any(u => u.Email == this.User.Identity.Name);
            if (user == false)
            {
                return RedirectToAction("Error", "StatsResponse");
            }

            var result =
                from s in db.StatsResponse
                group s by new { date = new DateTime(s.UserDate.Year, s.UserDate.Month, 1) } into g
                select new StatsResponse
                {
                    UserDate = g.Key.date,
                    WeightAverage = Math.Round(g.Select(z => z.Weight).Average(), 2),
                    BMIAverage = Math.Round(g.Select(z => z.BMI).Average(), 2)
                };

            return View(result);
        }

        // GET: StatsResponse/Error
        public IActionResult Error()
        {
            return View();
        }
    }
}
