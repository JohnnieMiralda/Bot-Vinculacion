using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HoursTracker.Core.Students;
using Microsoft.AspNetCore.Mvc;

namespace HoursTracker.Web
{
    public class StudentsHoursReportController : Controller
    {
        private readonly IStudentService _studentService;

        public StudentsHoursReportController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult<IEnumerable<StudentsHoursReportDto>>> All(string account)
        {
            var data = (await _studentService.HoursByStudent(account)).ToList();
            return Ok(data);
        }
    }
}
