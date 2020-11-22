using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HoursTracker.Core.Classes;
using Microsoft.AspNetCore.Mvc;

namespace HoursTracker.Web.Controllers
{
    public class ProjectsClassReportController : Controller
    {
        private readonly IClassService _classService;

        public ProjectsClassReportController(IClassService classService)
        {
            _classService = classService;
        }

        public async Task<ActionResult<IEnumerable<ProjectsClassReportDto>>> All(string classCode)
        {
            var data = (await _classService.ProjectsByClass(classCode)).ToList();
            return Ok(data);
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
