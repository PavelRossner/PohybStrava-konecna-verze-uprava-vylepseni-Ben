using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PohybStrava.Data;
using PohybStrava.Models;

namespace PohybStrava.Controllers
{
    public class ActivitiesController : Controller
    {
        private readonly ApplicationDbContext db;

        public ActivitiesController(ApplicationDbContext context)
        {
            db = context;
        }

        // GET: Activities
        public async Task<IActionResult> Index()
        {
            var Id = User.Identity.GetUserId();
            
            IQueryable<Activity> activities = db.Activities.Where(u => u.UserId == Id || User.Identity.Name.Contains("admin"));

            bool user = db.User.Any(u => u.Email == this.User.Identity.Name);
            if (user == false)
            {
                return RedirectToAction("Error", "Activities");
            }

            return View(await db.Activities.OrderBy(d => d.DateActivity).ToList());
        }

        // GET: Activities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || db.Activities == null)
            {
                return NotFound();
            }

            var activities = await db.Activities
                .FirstOrDefaultAsync(m => m.ActivityId == id);
            if (activities == null)
            {
                return NotFound();
            }

            return View(activities);
        }

        // GET: Activities/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Activities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ActivitiesId,UserId,Email,DateActivities,Trail,Distance,Elevation,Time,Pace,EnergyActivities,Day,Month,Year,DistanceSum,ElevationSum,EnergyActivitiesTotal")] Activity activities, User user)
        {
            if (ModelState.IsValid)
            {
                user = db.User.FirstOrDefault(u => u.Email == this.User.Identity.Name);
                activities.UserId= user.Id;

                db.Add(activities);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(activities);
        }

        // GET: Activities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || db.Activities == null)
            {
                return NotFound();
            }

            var activities = await db.Activities.FindAsync(id);
            if (activities == null)
            {
                return NotFound();
            }
            return View(activities);
        }

        // POST: Activities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ActivitiesId,UserId,Email,DateActivities,Trail,Distance,Elevation,Time,Pace,EnergyActivities,Day,Month,Year,DistanceSum,ElevationSum,EnergyActivitiesTotal")] Activity activities)
        {
            if (id != activities.ActivityId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(activities);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActivitiesExists(activities.ActivityId))
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
            return View(activities);
        }

        // GET: Activities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || db.Activities == null)
            {
                return NotFound();
            }

            var activities = await db.Activities
                .FirstOrDefaultAsync(m => m.ActivityId == id);
            if (activities == null)
            {
                return NotFound();
            }

            return View(activities);
        }

        // POST: Activities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (db.Activities == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Activities'  is null.");
            }
            var activities = await db.Activities.FindAsync(id);
            if (activities != null)
            {
                db.Activities.Remove(activities);
            }
            
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ActivitiesExists(int id)
        {
          return db.Activities.Any(e => e.ActivityId == id);
        }


        // Activities - Day Overview
        public ActionResult SumActivitiesDay()
        {
            bool user = db.User.Any(u => u.Email == this.User.Identity.Name);
            if (user == false)
            {
                return RedirectToAction("Error", "Activities");
            }

            var result =
                from s in db.Activities
                group s by new { date = new DateTime(s.DateActivity.Year, s.DateActivity.Month, s.DateActivity.Day) } into g
                select new Activity
                {
                    DateActivity = g.Key.date,
                    DistanceSum = (int)g.Sum(x => x.Distance),
                    ElevationSum = (int)g.Sum(y => y.Elevation),
                    EnergyActivityTotal = (int)g.Sum(z => z.EnergyActivity)
                };

            return View(result);
        }

        //Activities - Month Overview
        public IActionResult MonthActivitiesOverview()
        {
            bool user = db.User.Any(u => u.Email == this.User.Identity.Name);
            if (user == false)
            {
                return RedirectToAction("Error", "Activities");
            }

            var result =
               from s in db.Activities
               group s by new { date = new DateTime(s.DateActivity.Year, s.DateActivity.Month, 1) } into g
               select new Activity
               {
                   DateActivity = g.Key.date,
                   DistanceSum = (int)g.Sum(x => x.Distance),
                   ElevationSum = (int)g.Sum(y => y.Elevation),
                   EnergyActivityTotal = (int)g.Sum(z => z.EnergyActivity)
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
