using System;
using System.Linq;
using System.Threading.Tasks;
using HoursTracker.Core.VinculationTypes;
using HoursTracker.Domain.Aggregates.VinculationTypes;
using HoursTracker.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HoursTracker.Web.Controllers
{
    public class VinculationTypesController : Controller
    {
        private readonly IVinculationTypeService _vinculationTypeService;

        public VinculationTypesController(IVinculationTypeService vinculationTypeService)
        {
            _vinculationTypeService = vinculationTypeService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            var data = (await _vinculationTypeService.All())
                .Select(vType => new VinculationTypeViewModel
                {
                    Id = vType.Id,
                    Code = vType.Code,
                    Type = vType.Type,
                    
                });

            return Ok(data);
        }

        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _vinculationTypeService.FindById(id));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(VinculationTypeViewModel vinculationTypeViewModel)
        {
            var existingCampus = await _vinculationTypeService.FindByCode(vinculationTypeViewModel.Code);

            if (existingCampus == null)
            {
                var vType = new VinculationType
                {
                    Code = vinculationTypeViewModel.Code,
                    Type = vinculationTypeViewModel.Type
                    
                };
                await _vinculationTypeService.Create(vType);
                return Ok();
            }
            else
            {
                return BadRequest("Ya existe un tipo de vinculación con este codigo");
            }

        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task Delete(int id)
        {
            await _vinculationTypeService.Remove(id);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            return View(id);
        }

        [HttpPut]
        public async Task<ActionResult> Edit(int id, VinculationTypeViewModel vinculationTypeViewModel)
        {
            var temp = await _vinculationTypeService.FindById(id);
            var existingVType = await _vinculationTypeService.FindByCode(vinculationTypeViewModel.Code);
            if (existingVType == null || temp.Code == existingVType.Code)
            {
                var vType = new VinculationType
                {
                    Code = vinculationTypeViewModel.Code,
                    Type = vinculationTypeViewModel.Type
                    
                };
                await _vinculationTypeService.Update(id, vType);
                return Ok();
            }
            else
            {
                return BadRequest("Ya existe un tipo de vinculación con este codigo");
            }

        }

    }
}
