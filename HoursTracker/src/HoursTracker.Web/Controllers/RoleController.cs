using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HoursTracker.Web.Areas.Identity.Data;
using HoursTracker.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HoursTracker.Web.Controllers
{
    public class RoleController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public RoleController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        private DataContext db = new DataContext();

        /* Code to assign the role of admin to the current log in user
        public async Task<IActionResult> Index()
        {

            if (User.Identity.IsAuthenticated)
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
                var user = await userManager.GetUserAsync(HttpContext.User);
                await userManager.AddToRoleAsync(user, "Admin");
            }

            return View();
        }
        */

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.AspNetUsers = db.AspNetUsers.ToList();
            return View();
        }

        public async Task<IActionResult> Create()
        {

            if (User.Identity.IsAuthenticated)
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
                var user = await userManager.GetUserAsync(HttpContext.User);
                await userManager.AddToRoleAsync(user, "Admin");
            }

            return Ok();
        }


    }
}
