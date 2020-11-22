using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HoursTracker.Core.Careers;
using HoursTracker.Domain.Aggregates.Careers;
using HoursTracker.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HoursTracker.Web.Controllers
{
    public class CareersController : Controller
    {
        private readonly ICareerService _careerService;

        public CareersController(ICareerService careerService)
        {
            _careerService = careerService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            
            var data = (await _careerService.All())
                .Select(career => new CareerViewModel
                {
                    Id = career.Id,
                    Name = career.Name,
                    Code = career.Code,
                    FacultyName = career.Faculty,
                });;

            return Ok(data);
        }

        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _careerService.FindById(id));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public async Task<ActionResult> Create(CareerViewModel careerViewModel)
        {
            var existingCode = await _careerService.FindByCode(careerViewModel.Code);
            if (existingCode == null)
            {
                var career = new Career
                {
                    Name = careerViewModel.Name,
                    Code = careerViewModel.Code,
                    FacultyId = careerViewModel.FacultyId,
                };

                await _careerService.Create(career);
                return Ok();
            }
            else
            {
                return BadRequest("Ya existe una carrera con ese codigo");
            }
            
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task Delete(int id)
        {
            await _careerService.Remove(id);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            return View(id);
        }

        [HttpPut]
        public async Task<ActionResult> Edit(int id, CareerViewModel careerViewModel)
        {
            var temp = await _careerService.FindById(id);
            var existingCode = await _careerService.FindByCode(careerViewModel.Code);
            if (existingCode == null || existingCode.Code == temp.Code)
            {
                var career = new Career
                {
                    Name = careerViewModel.Name,
                    Code = careerViewModel.Code,
                    FacultyId = careerViewModel.FacultyId
                };
                await _careerService.Update(id, career);
                return Ok();
            }
            else
            {
                return BadRequest("Ya existe una carrera con ese codigo");
            }
            
        }
    }
}