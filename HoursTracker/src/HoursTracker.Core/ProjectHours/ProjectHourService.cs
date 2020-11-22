using HoursTracker.Domain.Aggregates.ProjectHours;
using HoursTracker.Domain.Aggregates.Projects;
using HoursTracker.Domain.Aggregates.Sections;
using HoursTracker.Domain.Aggregates.Students;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoursTracker.Core.ProjectHours
{
    public class ProjectHourService : IProjectHourService
    {
        private readonly IProjectHourRepository _projecthourRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly ISectionRepository _sectionRepository;
        private readonly IProjectRepository _projectRepository;

        public ProjectHourService(IProjectHourRepository projecthourRepository, IStudentRepository studentRepository, ISectionRepository sectionRepository, IProjectRepository projectRepository)
        {
            _projecthourRepository = projecthourRepository;
            _studentRepository = studentRepository;
            _sectionRepository = sectionRepository;
            _projectRepository = projectRepository;
        }

        public async Task<IEnumerable<SingleProjectHourDto>> All()
        {
            return await _projecthourRepository
                 .Filter(projecthour => !projecthour.Disabled)
                 .Select(projecthour => new SingleProjectHourDto
                 {
                     Id = projecthour.Id,
                     Hours = projecthour.Hours,
                     TableState = projecthour.TableState,
                     StudentAccount = projecthour.Student.Account,
                     StudentFirstName = projecthour.Student.FirstName,
                     StudentLastName = projecthour.Student.FirstSurname,
                     ProjectName = projecthour.Project.Name,
                     SectionCode = projecthour.Section.Code
                 })
                 .ToListAsync();
        }

        public async Task Create(CreateProjectHourDto projecthour)
        {
            //var student = await _studentRepository.FindById(projecthour.Student);
            var student = await _studentRepository.FirstOrDefault(student => student.Account == projecthour.Student);
            var section = await _sectionRepository.FirstOrDefault(section => section.Id == projecthour.Section);
            var project = await _projectRepository.FirstOrDefault(project => project.Id == projecthour.Project);

            var projecthourInfo = new ProjectHour
            {
                Hours = projecthour.Hours,
                TableState = projecthour.TableState,
                Student = student,
                StudentId = student.Id,
                Section = section,
                SectionId = section.Id,
                Project = project,
                ProjectId = project.Id
            };
            await _projecthourRepository.Add(projecthourInfo);
        }

        public async Task<ProjectHour> FindById(int id)
        {
            return await _projecthourRepository.FindById(id);
        }

        public async Task Remove(int id)
        {
            var projecthour = await _projecthourRepository.FindById(id);
            await _projecthourRepository.Disable(projecthour);
        }

        public async Task Accept(int id)
        {
            var projecthour = await _projecthourRepository.FindById(id);

            if(projecthour.TableState == 0)
            {
                projecthour.TableState = 1;
                await _projecthourRepository.Update(projecthour);
            }
       
        }

        public async Task Update(int id, UpdateProjectHourDto projecthour)
        {
            var existingProjectHour = await _projecthourRepository.FindById(id);
            var student = await _studentRepository.FirstOrDefault(student => student.Id == existingProjectHour.StudentId); 
            var section = await _sectionRepository.FirstOrDefault(section => section.Id == projecthour.Section);
            var project = await _projectRepository.FirstOrDefault(project => project.Id == projecthour.Project);


            existingProjectHour.Hours = projecthour.Hours;
            existingProjectHour.TableState = projecthour.TableState;
            existingProjectHour.Student = student;
            existingProjectHour.Section = section;
            existingProjectHour.Project = project;

            await _projecthourRepository.Update(existingProjectHour);
        }
    }
}
