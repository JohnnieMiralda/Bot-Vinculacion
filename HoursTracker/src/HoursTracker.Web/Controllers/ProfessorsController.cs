using System.Linq;
using System.Threading.Tasks;
using HoursTracker.Core.Professors;
using HoursTracker.Domain.Aggregates.Professors;
using HoursTracker.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HoursTracker.Web.Controllers
{
    public class ProfessorsController : Controller
    {
        private readonly IProfessorService _professorService;

        public ProfessorsController(IProfessorService professorService)
        {
            _professorService = professorService;
        }
        
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        
        [HttpGet]
        public async Task<IActionResult> All()
        {
            var data = (await _professorService.All())
                .Select(professor => new ProfessorViewModel
                {
                    Id = professor.Id,
                    Code = professor.Code,
                    FirstName = professor.FirstName,
                    SecondName = professor.SecondName,
                    FirstLastName = professor.FirstLastName,
                    SecondLastName = professor.SecondLastName
                });
            
            return Ok(data);
        }
        
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _professorService.FindById(id));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(ProfessorViewModel professorViewModel)
        {
            var existingCode = await _professorService.FindByCode(professorViewModel.Code);
            if (existingCode == null) {
                var professor = new Professor
                {
                    Code = professorViewModel.Code,
                    FirstName = professorViewModel.FirstName,
                    SecondName = professorViewModel.SecondName,
                    FirstLastName = professorViewModel.FirstLastName,
                    SecondLastName = professorViewModel.SecondLastName
                };

                await _professorService.Create(professor);
                return Ok();
            }
            else
            {
                return BadRequest("Ya existe un Maestro con este codigo");
            }
            
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task Delete(int id)
        {
            await _professorService.Remove(id);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            return View(id);
        }
        
        [HttpPut]
        public async Task<ActionResult> Edit(int id, ProfessorViewModel professorViewModel)
        {
            var temp = await _professorService.FindById(id);
            var existingCode = await _professorService.FindByCode(professorViewModel.Code);
            if (existingCode == null || temp.Code == existingCode.Code) {
                var professor = new Professor
                {
                    Code = professorViewModel.Code,
                    FirstName = professorViewModel.FirstName,
                    SecondName = professorViewModel.SecondName,
                    FirstLastName = professorViewModel.FirstLastName,
                    SecondLastName = professorViewModel.SecondLastName
                };
                await _professorService.Update(id, professor);
                return Ok();
            }
            else
            {
                return BadRequest("Ya existe un Maestro con este codigo");
            }
            
        }
    }
}