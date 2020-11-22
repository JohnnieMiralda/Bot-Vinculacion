using HoursTracker.Domain.Aggregates.Faculties;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoursTracker.Core.Faculties
{
    public class FacultyService : IFacultyService
    {
        private readonly IFacultyRepository _facultyRepository;
        public FacultyService(IFacultyRepository facultyRepository)
        {
            _facultyRepository = facultyRepository;
        }

        public async Task<IEnumerable<Faculty>> All()
        {
            return await _facultyRepository
                .Filter(faculty => !faculty.Disabled)
                .ToListAsync();
        }

        public async Task<Faculty> FindByCode(string code)
        {
            return await _facultyRepository.FirstOrDefault(c => c.Code == code);
        }
        public async Task Create(Faculty faculty)
        {
            await _facultyRepository.Add(faculty);
        }

        public async Task<Faculty> FindById(int id)
        {
            return await _facultyRepository.FindById(id);
        }

        public async Task Remove(int id)
        {
            var faculty = await _facultyRepository.FindById(id);
            await _facultyRepository.Disable(faculty);
        }

        public async Task Update(int id, Faculty faculty)
        {
            var existingFaculty = await _facultyRepository.FindById(id);

            existingFaculty.Code = faculty.Code;
            existingFaculty.Name = faculty.Name;

            await _facultyRepository.Update(existingFaculty);
        }

        public async Task<IEnumerable<HoursFacultiesReportDto>> HoursFaculty(string code)
        {
            return await _facultyRepository
                .Filter(x => x.Code.Equals(code))
                .SelectMany(faculty => faculty.Careers,
                    (faculty, career) => new
                    {
                        FacultyCode = faculty.Code,
                        FacultyName = faculty.Name,
                        ClassCarrer = career.ClassCareers
                    })
                .SelectMany(x => x.ClassCarrer, (faculty, @class) => new
                {
                    faculty.FacultyCode,
                    faculty.FacultyName,
                    @class.Class.ClassName,
                    @class.Class.ClassCode,
                    @class.Class.Sections
                })
                .SelectMany(x => x.Sections, (faculty, sections) => new
                {
                    faculty.FacultyCode,
                    faculty.FacultyName,
                    faculty.ClassName,
                    faculty.ClassCode,
                    sections.ProjectHours
                })
                .SelectMany(x => x.ProjectHours, (faculty, hours) => new HoursFacultiesReportDto
                {
                    FacultyCode = faculty.FacultyCode,
                    FacultyName = faculty.FacultyName,
                    ClassName = faculty.ClassName,
                    ClassCode = faculty.ClassCode,
                    HoursAmount = hours.Hours,
                    ProjectCode = hours.Project.Code,
                    ProjectName = hours.Project.Name

                })
                .ToListAsync();
        }
    }
}
