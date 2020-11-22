using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HoursTracker.Core.Students;
using HoursTracker.Domain.Aggregates.Campuses;
using HoursTracker.Domain.Aggregates.Careers;
using HoursTracker.Domain.Aggregates.Students;
using HoursTracker.Domain.Shared;
using HoursTracker.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace HoursTracker.Web.Controllers
{
    public class StudentsController : Controller
    {
        private readonly IStudentService _studentService;

        public StudentsController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            var data = (await _studentService.All())
                .Select(student => new SingleStudentViewModel
                {
                    Id = student.Id,
                    Account = student.Account,
                    FirstName = student.FirstName,
                    SecondName = student.SecondName,
                    FirstSurname = student.FirstSurname,
                    SecondSurname = student.SecondSurname,
                    Settlement = student.Settlement,
                    CareerName = student.CareerName,
                    CampusName = student.CampusName,
                    Email = student.Email,
                    isInBot = student.isInBot,
                    TelegramAccount = student.TelegramAccount
                }); ;

            return Ok(data);
        }

        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _studentService.FindById(id));
        }

        public async Task<IActionResult> FindByAccount(string id)
        {
            
            
            try
            {
                var student = await _studentService.FindByAccount(id);
                return Ok(student);
            }
            catch (System.Exception)
            {

                return BadRequest("Invalid account");
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateStudentViewModel studentViewModel)
        {
            var existingCode = await _studentService.FindByCode(studentViewModel.Account);

            if (existingCode == null)
            {
                var student = new CreateStudentDto
                {
                    Account = studentViewModel.Account,
                    FirstName = studentViewModel.FirstName,
                    SecondName = studentViewModel.SecondName,
                    FirstSurname = studentViewModel.FirstSurname,
                    SecondSurname = studentViewModel.SecondSurname,
                    Campus = studentViewModel.Campus,
                    Careers = studentViewModel.Careers,
                    Settlement = studentViewModel.Settlement,
                    Email = studentViewModel.Email
                };

                await _studentService.Create(student);
                return Ok();
            }
            else
            {
                return BadRequest("Ya existe un Estudiante con este numero de Cuenta");
            }
            

        }

        [HttpDelete]
        public async Task Delete(int id)
        {
            await _studentService.Remove(id);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            return View(id);
        }

        [HttpPut]
        public async Task<ActionResult> Edit(int id, CreateStudentViewModel studentViewModel)
        {
            var temp = await _studentService.FindById(id);
            var existingCode = await _studentService.FindByCode(studentViewModel.Account);

            if (existingCode == null || existingCode.Account == temp.Account)
            {
                var student = new UpdateStudentDto
                {
                    Account = studentViewModel.Account,
                    Email = studentViewModel.Email,
                    Campus = studentViewModel.Campus,
                    FirstName = studentViewModel.FirstName,
                    SecondName = studentViewModel.SecondName,
                    FirstSurname = studentViewModel.FirstSurname,
                    SecondSurname = studentViewModel.SecondSurname,
                    Careers = studentViewModel.Careers,
                    Settlement = studentViewModel.Settlement,
                    Id = id
                };
                await _studentService.Update(id, student);
                return Ok();
            }
            else 
            {
                return BadRequest("Ya existe un Estudiante con este numero de Cuenta");
            }
            
        }
    }
}