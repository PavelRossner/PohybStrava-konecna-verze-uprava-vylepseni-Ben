using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using PohybStrava.Data;
using PohybStrava.Models;
using PohybStrava.Models.Response;
using static Microsoft.AspNetCore.Razor.Language.TagHelperMetadata;

namespace PohybStrava.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class EnergyBalanceController : Controller
    {
        private readonly ApplicationDbContext db;

        public EnergyBalanceController(ApplicationDbContext context)
        {
            db = context;
        }

        // GET: EnergyBalance
        [HttpGet]
        public IActionResult Index()
        {
            bool user = db.User.Any(u => u.Email == this.User.Identity.Name);
            
            if (user == false)
            {
                return RedirectToAction("Error", "EnergyBalance");
            }

            List<DateTime> dateTimes = new List<DateTime>();
            string databaseUser = User.Identity.GetUserId();

            // Get all activities for current user and from those activities select datumactivities
            List<DateTime> activityDateTimes = db.Activity.Where(u => u.UserId == databaseUser).Select(a => a.DateActivity).ToList();

            // Get all diets for current user and from those diets select datumdiets
            List<DateTime> dietDateTimes = db.Diet.Where(u => u.UserId == databaseUser).Select(a => a.DateDiet).ToList();

            // Add resulting datetimes to one collection
            dateTimes.AddRange(activityDateTimes);
            dateTimes.AddRange(dietDateTimes);

            // Trim hours, minutes, seconds from all datetimes and remove repeating ones (excess datetimes)
            dateTimes = dateTimes.Select(d => d.Date).Distinct().ToList();

            // Iterate through resulting datetimes and create list of EnergyBalance
            List<EnergyBalanceResponse> energyBalance = dateTimes.Select(dt =>
            {
                EnergyBalanceResponse output = new EnergyBalanceResponse();

                output.DietDate = dt;
                output.ActivityDate = dt;


                // Get all activities for the date and current user and for those activities calculate sum of Energie
                output.EnergyActivitesTotal = (int)db.Activity.Where(a => a.UserId == databaseUser && a.DateActivity.Day == dt.Day && a.DateActivity.Month == dt.Month && a.DateActivity.Year == dt.Year)
                                                                         .Sum(a => a.EnergyActivity);

                // Get all diet for the date and current user and for those diets calculate sum of celkem
                output.EnergyDietTotal = (int)db.Diet.Where(d => d.UserId == databaseUser && d.DateDiet.Day == dt.Day && d.DateDiet.Month == dt.Month && d.DateDiet.Year == dt.Year)
                                                             .Sum(d => d.EnergyDiet);

                // Get all athletes for the date and current user and for those subjects calculate sum of basal metabolism
                output.BMR = db.Stats.Where(u => u.UserId == databaseUser && u.UserDate.Day == dt.Day && u.UserDate.Month == dt.Month && u.UserDate.Year == dt.Year)
                                          .Sum(u => Math.Round(655.0955 + 9.5634 * u.Weight + 1.8496 * u.Height - 4.6756 * u.Age, 0));


                //DODELAT POHLAVI

                // Return the resulting PrijemVydej
                return output;
            }).ToList();

            EnergyBalanceListResponse response = new EnergyBalanceListResponse
            {
                EnergyBalanceList = energyBalance
            };

            return View(response.EnergyBalanceList.OrderBy(d => d.DietDate));

        }

    

        // GET: EnergyBalance/Error
        public IActionResult Error()
        {
            return View(); ;
        }
    }
}
