using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HoursTracker.Core.ProjectHours;
using HoursTracker.Domain.Aggregates.ProjectHours;
using HoursTracker.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HoursTracker.Web.Controllers
{
    public class ProjectHoursController : Controller
    {
        private readonly IProjectHourService _projecthourService;

        public ProjectHoursController(IProjectHourService projecthourService)
        {
            _projecthourService = projecthourService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> All()
        {
            var data = (await _projecthourService.All())
                .Select(projecthour => new ProjectHourViewModel
                {
                    Id = projecthour.Id,
                    TableState = projecthour.TableState,
                    Hours = projecthour.Hours,
                    StudentAccount = projecthour.StudentAccount,
                    StudentFirstName = projecthour.StudentFirstName,
                    StudentLastName = projecthour.StudentLastName,
                    ProjectName = projecthour.ProjectName,
                    SectionCode = projecthour.SectionCode
                });
            return Ok(data);
        }

        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _projecthourService.FindById(id));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task Create(CreateProjectHourViewModel projecthourViewModel)
        {
            var projecthour = new CreateProjectHourDto
            {
                TableState = projecthourViewModel.TableState,
                Hours = projecthourViewModel.Hours,
                Student = projecthourViewModel.Student,
                Section = projecthourViewModel.Section,
                Project = projecthourViewModel.Project
            };

            await _projecthourService.Create(projecthour);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task Delete(int id)
        {
            await _projecthourService.Remove(id);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [HttpPut]
        public async Task Approve(int id)
        {
            await _projecthourService.Accept(id);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            return View(id);
        }

        [HttpPut]
        public async Task Edit(int id, CreateProjectHourViewModel projecthourViewModel)
        {
            var projecthour = new UpdateProjectHourDto()
            {
                TableState = projecthourViewModel.TableState,
                Hours = projecthourViewModel.Hours,
                Student = projecthourViewModel.Student,
                Section = projecthourViewModel.Section,
                Project = projecthourViewModel.Project
            };
            await _projecthourService.Update(id, projecthour);
        }

    }
  
}