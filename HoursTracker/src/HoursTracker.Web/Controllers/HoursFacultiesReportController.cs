using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HoursTracker.Core.Faculties;
using Microsoft.AspNetCore.Mvc;

namespace HoursTracker.Web
{
    public class HoursFacultiesReportController : Controller
    {
        private readonly IFacultyService _facultyService;


        public HoursFacultiesReportController(IFacultyService facultyService)
        {
            _facultyService = facultyService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult<IEnumerable<HoursFacultiesReportDto>>> All(string code)
        {
            var data = (await _facultyService.HoursFaculty(code)).ToList();
            return Ok(data);
        }
    }
}
