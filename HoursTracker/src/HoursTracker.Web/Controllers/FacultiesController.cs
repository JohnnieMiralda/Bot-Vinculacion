using HoursTracker.Core.Faculties;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HoursTracker.Core.Careers;
using HoursTracker.Domain.Aggregates.Careers;
using HoursTracker.Web.Models;
using Microsoft.AspNetCore.Mvc;
using HoursTracker.Domain.Aggregates.Faculties;
using Microsoft.AspNetCore.Authorization;

namespace HoursTracker.Web.Controllers
{
    public class FacultiesController : Controller
    {
        private readonly IFacultyService _facultyService;

        public FacultiesController(IFacultyService facultyService)
        {
            _facultyService = facultyService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            var data = (await _facultyService.All())
                .Select(faculty => new FacultyViewModel
                {
                    Id = faculty.Id,
                    Code = faculty.Code,
                    Name = faculty.Name
                });

            return Ok(data);
        }

        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _facultyService.FindById(id));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(FacultyViewModel facultyViewModel)
        {
            var existingCode = await _facultyService.FindByCode(facultyViewModel.Code);
            if (existingCode == null)
            {
                var faculty = new Faculty
                {
                    Code = facultyViewModel.Code,
                    Name = facultyViewModel.Name
                };

                await _facultyService.Create(faculty);
                return Ok();
            }
            else
            {
                return BadRequest("Ya existe una Facultad con este codigo");
            }

        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task Delete(int id)
        {
            await _facultyService.Remove(id);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            return View(id);
        }

        [HttpPut]
        public async Task<ActionResult> Edit(int id, FacultyViewModel facultyViewModel)
        {
            var temp = await _facultyService.FindById(id);
            var existingCode = await _facultyService.FindByCode(facultyViewModel.Code);
            if (existingCode == null || temp.Code == existingCode.Code)
            {
                var faculty = new Faculty
                {
                    Code = facultyViewModel.Code,
                    Name = facultyViewModel.Name
                };
                await _facultyService.Update(id, faculty);
                return Ok();
            }
            else
            {
                return BadRequest("Ya existe una Facultad con este codigo");
            }

        }
    }
}
