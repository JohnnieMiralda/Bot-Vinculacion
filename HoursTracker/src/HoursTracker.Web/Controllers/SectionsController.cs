using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HoursTracker.Core.Sections;
using HoursTracker.Domain.Aggregates.Sections;
using HoursTracker.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HoursTracker.Web.Controllers
{
    public class SectionsController : Controller
    {
        private readonly ISectionService _sectionService;

        public SectionsController(ISectionService sectionService)
        {
            _sectionService = sectionService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            var data = (await _sectionService.All())
                .Select(section => new SectionsViewModel
                {
                    Id = section.Id,
                    ClassName = section.Class,
                    Code = section.Code,
                    Period = section.Period,
                    Professor = section.Professor
                });

            return Ok(data);
        }

        [HttpGet]
        public async Task<IActionResult> StudentsBySection(int id)
        {
            var data = (await _sectionService.FindStudentsBySection(id));

            return Ok(data);
        }

        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _sectionService.FindById(id));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateSectionViewModel sectionViewModel)
        {
            var existingCode = await _sectionService.FindByCode(sectionViewModel.Code);

            if(existingCode == null)
            {
                var section = new CreateSectionDto
                {
                    Code = sectionViewModel.Code,
                    Class = sectionViewModel.Class,
                    Period = sectionViewModel.Period,
                    Professor = sectionViewModel.Professor,
                    Students = sectionViewModel.Students
                };
                await _sectionService.Create(section);
                return Ok();
            }
            else
            {
                return BadRequest("Ya existe una seccion con este codigo");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task Delete(int id)
        {
            await _sectionService.Remove(id);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            return View(id);
        }

        [HttpPut]
        public async Task<ActionResult> Edit(int id, CreateSectionViewModel sectionViewModel)
        {
            var temp = await _sectionService.FindById(id);
            var existingCode = await _sectionService.FindByCode(sectionViewModel.Code);

            if (existingCode == null || existingCode.Code == temp.Code)
            {
                var section = new UpdateSectionDto()
                {
                    Code = sectionViewModel.Code,
                    Class = sectionViewModel.Class,
                    Period = sectionViewModel.Period,
                    Professor = sectionViewModel.Professor,
                    Students = sectionViewModel.Students
                };
                await _sectionService.Update(id, section);
                return Ok();
            }
            else
            {
                return BadRequest("Ya existe una seccion con este codigo");
            }
        }
    }
}