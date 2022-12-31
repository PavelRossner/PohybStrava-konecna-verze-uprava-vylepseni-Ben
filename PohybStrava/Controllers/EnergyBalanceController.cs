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

        public EnergyBalanceController(ApplicationDbContext db)
        {
            this.db = db;
        }

        // GET: EnergyBalance
        [HttpGet]
        public IActionResult Index()
        {            
            string Id = User.Identity.GetUserId();
            User user = db.Users.FirstOrDefault(u => u.Id == Id);

            if (user == null)
            {
                return RedirectToAction("Error", "EnergyBalance");
            }

            List<DateTime> dateTimes = new List<DateTime>();
            //string databaseUser = User.Identity.GetUserId();

            // Get all activities for current user and from those activities select datumactivities
            List<DateTime> activityDateTimes = db.Activity.Where(u => u.UserId == Id).Select(a => a.DateActivity).ToList();

            // Get all diets for current user and from those diets select datumdiets
            List<DateTime> dietDateTimes = db.Diet.Where(u => u.UserId == Id).Select(d => d.DateDiet).ToList();

            // Get all user inputs and from those select datumusers
            List<DateTime> statsDateTimes = db.Stats.Where(u => u.UserId == Id).Select(s => s.DateUser).ToList();

            // Add resulting datetimes to one collection
            dateTimes.AddRange(activityDateTimes);
            dateTimes.AddRange(dietDateTimes);
            dateTimes.AddRange(statsDateTimes);

            // Trim hours, minutes, seconds from all datetimes and remove repeating ones (excess datetimes)
            dateTimes = dateTimes.Select(d => d.Date).Distinct().ToList();

            // Iterate through resulting datetimes and create list of EnergyBalance
            List<EnergyBalanceResponse> energyBalance = dateTimes.Select(dt =>
            {
                EnergyBalanceResponse output = new EnergyBalanceResponse();

                StatsResponse gender = new StatsResponse();

                output.DietDate = dt;
                output.ActivityDate = dt;
                output.UserDate= dt;

                // Get all activities for the date and current user and for those activities calculate sum of Energie
                output.EnergyActivitesTotal = (int)db.Activity.Where(a => a.UserId == Id && a.DateActivity.Day == dt.Day && a.DateActivity.Month == dt.Month && a.DateActivity.Year == dt.Year)
                                                                         .Sum(a => a.EnergyActivity);

                // Get all diet for the date and current user and for those diets calculate sum of celkem
                output.EnergyDietTotal = (int)db.Diet.Where(d => d.UserId == Id && d.DateDiet.Day == dt.Day && d.DateDiet.Month == dt.Month && d.DateDiet.Year == dt.Year)
                                                             .Sum(d => d.EnergyDiet);

                // Get all athletes for the date and current user and for those subjects calculate sum of basal metabolism
                if (gender.Gender == "žena")
                {
                    output.BMR = db.Stats.Where(u => u.UserId == Id && u.DateUser.Day == dt.Day && u.DateUser.Month == dt.Month && u.DateUser.Year == dt.Year)
                                        .Select(StatsResponse.GetStatsResponse)
                                        .Sum(u => Math.Round(655.0955 + 9.5634 * u.Weight + 1.8496 * u.Height - 4.6756 * u.Age, 0));
                }

                else
                {
                    output.BMR = db.Stats.Where(u => u.UserId == Id && u.DateUser.Day == dt.Day && u.DateUser.Month == dt.Month && u.DateUser.Year == dt.Year)
                                         .Select(StatsResponse.GetStatsResponse)
                                         .Sum(u => Math.Round(66.473 + 13.7516 * u.Weight + 5.0033 * u.Height - 6.755 * u.Age, 0));
                }

                // Return the resulting PrijemVydej
                return output;
            }).ToList();

            EnergyBalanceListResponse response = new EnergyBalanceListResponse
            {
                EnergyBalanceList = energyBalance
            };

            return View(response.EnergyBalanceList.OrderBy(d => d.UserDate));
        }

    

        // GET: EnergyBalance/Error
        public IActionResult Error()
        {
            return View(); ;
        }
    }
}
