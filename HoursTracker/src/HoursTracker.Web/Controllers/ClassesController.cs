using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HoursTracker.Core.Classes;
using HoursTracker.Domain.Aggregates.Classes;
using HoursTracker.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace HoursTracker.Web.Controllers
{
    public class ClassesController : Controller
    {
        private readonly IClassService _classService;

        public ClassesController(IClassService classService)
        {
            _classService = classService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            var data = (await _classService.All());
                var data2 = data
                .Select(@class => new ClassViewModel
                {
                    Id = @class.Id,
                    ClassCode = @class.ClassCode,
                    ClassName = @class.ClassName,
                    CareerNames = @class.CareerNames,
                    Careers = @class.Careers               
                });

            return Ok(data2);
        }

        [HttpGet]
        public async Task<IActionResult> SingleClasses()
        {
            var data = (await _classService.SingleClasses());
            var data2 = data
            .Select(@class => new ClassViewModel
            {
                Id = @class.Id,
                ClassCode = @class.ClassCode,
                ClassName = @class.ClassName,
            });

            return Ok(data2);
        }

        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _classService.FindById(id));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(ClassViewModel classViewModel)
        {
            var existingCode = await _classService.FindByCode(classViewModel.ClassCode);
            if (existingCode == null)
            {
                var @class = new CreateClassDto
                {
                    ClassCode = classViewModel.ClassCode,
                    ClassName = classViewModel.ClassName,
                    Careers = classViewModel.Careers
                };

                await _classService.Create(@class);
                return Ok();
            }
            else
            {
                return BadRequest("Ya existe una clase con este codigo");
            }
           
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task Delete(int id)
        {
            await _classService.Remove(id);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            return View(id);
        }

        [HttpPut]
        public async Task<ActionResult> Edit(int id, ClassViewModel classViewModel)
        {
            var temp = await _classService.FindById(id);
            var existingCode = await _classService.FindByCode(classViewModel.ClassCode);
            if (existingCode == null || existingCode.ClassCode == temp.ClassCode) {
                var updatedClass = new UpdateClassDto
                {
                    ClassCode = classViewModel.ClassCode,
                    ClassName = classViewModel.ClassName,
                    Careers = classViewModel.Careers
                };
                await _classService.Update(id, updatedClass);
                return Ok();
            }
            else
            {
                return BadRequest("Ya existe una clase con este codigo");
            }
            
        }
    }
}
