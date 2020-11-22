using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HoursTracker.Core.Campuses;
using HoursTracker.Domain.Aggregates.Campuses;
using HoursTracker.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HoursTracker.Web.Controllers
{
    public class CampusesController : Controller
    {
        private readonly ICampusService _campusService;

        public CampusesController(ICampusService campusService)
        {
            _campusService = campusService;
        }

        
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            var data = (await _campusService.All())
                .Select(campus => new CampusViewModel
                {
                    Id = campus.Id,
                    Name = campus.Name,
                    Code = campus.Code,
                });

            return Ok(data);
        }

        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _campusService.FindById(id));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(CampusViewModel campusViewModel)
        {
            var existingCampus = await _campusService.FindByCode(campusViewModel.Code);
 
            
            if (existingCampus == null)
            {
                var campus = new Campus
                {
                    Name = campusViewModel.Name,
                    Code = campusViewModel.Code
                };
                await _campusService.Create(campus);
                return Ok();
            }
            else
            {
                return  BadRequest("Ya existe un campus con este codigo");
            }
            
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task Delete(int id)
        {
            await _campusService.Remove(id);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            return View(id);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<ActionResult> Edit(int id, CampusViewModel campusViewModel)
        {
            var temp = await _campusService.FindById(id);
            var existinCampus = await _campusService.FindByCode(campusViewModel.Code);
            if (existinCampus == null || temp.Code == existinCampus.Code)
            {
                var campus = new Campus
                {
                    Name = campusViewModel.Name,
                    Code = campusViewModel.Code
                };
                await _campusService.Update(id, campus);
                return Ok();
            }
            else
            {
                return BadRequest("Ya existe un campus con este codigo");
            }
            
        }
    }
}