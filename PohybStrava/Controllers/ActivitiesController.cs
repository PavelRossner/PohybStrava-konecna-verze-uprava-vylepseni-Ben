using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PohybStrava.Data;
using PohybStrava.Models;
using PohybStrava.Models.Response;

namespace PohybStrava.Controllers
{
    public class ActivitiesController : Controller
    {
        private readonly ApplicationDbContext db;

        public ActivitiesController(ApplicationDbContext db)
        {
            this.db = db;
        }

        // GET: Activities
        public async Task<IActionResult> Index()
        {
            string Id = User.Identity.GetUserId();
            User user = await db.User.FirstOrDefaultAsync(u => u.Id == Id);

            if (user == null)

            {
                return RedirectToAction("Error", "Activities");
            }

            if (User.Identity.Name.Contains("admin"))

            {
                return View(db.Activity.OrderBy(a => a.DateActivity)
                                         .Select(ActivityResponse.GetActivityResponse)
                                         .ToList());
            }

            else

            {
                return View(db.Activity.OrderBy(a => a.DateActivity)
                                         .Where(u => u.UserId == Id)
                                         .Select(ActivityResponse.GetActivityResponse)
                                         .ToList());
            }

        }

        // GET: Activities/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null || db.Activity == null)
            {
                return NotFound();
            }

            var activities = db.Activity.Select(ActivityResponse.GetActivityResponse)
                                          .FirstOrDefault(a => a.ActivityId == id);
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
        public async Task<IActionResult> Create([Bind("ActivityId,UserId,DateActivity,Trail,Distance,Elevation,Time,Pace,EnergyActivity")] Activity activity, User user)
        {
            if (ModelState.IsValid)
            {
                user = db.User.FirstOrDefault(u => u.Id == this.User.Identity.GetUserId());

                user.Activities.Add(activity);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Activities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || db.Activity == null)
            {
                return NotFound();
            }

            Activity activity = await db.Activity.FindAsync(id);

            if (activity == null)
            {
                return NotFound();
            }
            return View(activity);
        }

        // POST: Activities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Activity activity)
        {
            if (id != activity.ActivityId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(activity);
            }

            Activity dbActivity = this.db.Activity.FirstOrDefault(a => a.ActivityId == id);

            if (dbActivity == null)

            {
                return View(activity);
            }

            dbActivity.DateActivity = activity.DateActivity;
            dbActivity.Trail = activity.Trail;
            dbActivity.Distance = activity.Distance;
            dbActivity.Elevation = activity.Elevation;
            dbActivity.Time = activity.Time;
            dbActivity.Pace = activity.Pace;
            dbActivity.EnergyActivity = activity.EnergyActivity;

            db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        // GET: Activities/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null || db.Activity == null)
            {
                return NotFound();
            }

            ActivityResponse activities = db.Activity.Select(ActivityResponse.GetActivityResponse)
                .FirstOrDefault(a => a.ActivityId == id);

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
            if (db.Activity == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Activities'  is null.");
            }
            Activity activities = await db.Activity.FindAsync(id);
            if (activities != null)
            {
                db.Activity.Remove(activities);
            }

            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ActivitiesExists(int id)
        {
            return db.Activity.Any(e => e.ActivityId == id);
        }


        // Activities - Day Overview
        public IActionResult SumActivitiesDay(int? id)
        {
            bool user = db.Users.Any(u => u.Email == this.User.Identity.Name);
            if (user == false)
            {
                return RedirectToAction("Error", "Activities");
            }

            IEnumerable<ActivityResponse> activity = db.Activity.Select(ActivityResponse.GetActivityResponse);

            IEnumerable<ActivityResponse> result =
                from s in activity
                group s by new { date = new DateTime(s.DateActivity.Year, s.DateActivity.Month, s.DateActivity.Day) } into g
                select new Models.Response.ActivityResponse
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
            bool user = db.Users.Any(u => u.Email == this.User.Identity.Name);
            if (user == false)
            {
                return RedirectToAction("Error", "Activities");
            }

            IEnumerable<ActivityResponse> activity = db.Activity.Select(ActivityResponse.GetActivityResponse);

            IEnumerable<ActivityResponse> result =
               from s in activity      //přepsat do LINQ db.Activities.GroupBy...
               group s by new { date = new DateTime(s.DateActivity.Year, s.DateActivity.Month, 1) } into g
               select new Models.Response.ActivityResponse      //na vstupu selectu je kolekce                 //ActivityResponse je objekt
               {
                   DateActivity = g.Key.date,
                   DistanceSum = (int)g.Sum(x => x.Distance),
                   ElevationSum = (int)g.Sum(y => y.Elevation),
                   EnergyActivityTotal = (int)g.Sum(z => z.EnergyActivity)  //na výstupu je model datového typu Activity
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
