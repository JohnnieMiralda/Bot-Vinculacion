using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HoursTracker.Core.Periods;
using HoursTracker.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HoursTracker.Web.Controllers
{
    public class PeriodsController : Controller
    {
        private readonly IPeriodService _periodService;

        public PeriodsController(IPeriodService periodService)
        {
            _periodService = periodService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            var data = (await _periodService.All())
                .Select(period => new PeriodViewModel
                {
                    Id = period.Id,
                    Code = period.Code,
                    Semester = period.Semester,
                    Year = period.Year,
                    Trimester = period.Trimester
                }); ;

            return Ok(data);
        }

        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _periodService.FindById(id));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        /*
        [HttpPost]
        public async Task Create(CreateSectionViewModel sectionViewModel)
        {
            var section = new Period
            {

            };

            await _periodService.Create(section);

        }

        [HttpDelete]
        public async Task Delete(int id)
        {
            await _sectionService.Remove(id);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            return View(id);
        }

        [HttpPut]
        public async Task Edit(int id, CreateSectionViewModel sectionViewModel)
        {
            var section = new UpdateSectionDto()
            {

            };
            await _sectionService.Update(id, section);
        }*/
    }
}
