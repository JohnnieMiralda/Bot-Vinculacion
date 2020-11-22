using HoursTracker.Domain.Aggregates.Careers;
using HoursTracker.Domain.Aggregates.Classes;

using HoursTracker.Domain.Aggregates.Projects;
using HoursTracker.Domain.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoursTracker.Core.Classes
{
    public class ClassService : IClassService
    {
        private readonly IClassRepository _classRepository;
        private readonly ICareerRepository _careerRepository;

        public ClassService(IClassRepository classRepository, ICareerRepository careerRepository)
        {
            _classRepository = classRepository;
            _careerRepository = careerRepository;
        }

        public async Task<IEnumerable<SingleClassDto>> SingleClasses()
        {
            var data = await _classRepository
                .Filter(@class => !@class.Disabled)
                .AsNoTracking()
                .Select(x => new SingleClassDto
                {
                    ClassName = x.ClassName,
                    ClassCode = x.ClassCode,
                    Id = x.Id
                }
                )
                .AsNoTracking()
                 .ToListAsync();

            return data;
        }

        public async Task<IEnumerable<SingleClassDto>> All()
        {
            var data = await _classRepository
                .Filter(@class => !@class.Disabled)
                .Include(@class => @class.ClassCareers)
                .ThenInclude(c => c.Career)
                .SelectMany(@class => @class.ClassCareers.DefaultIfEmpty(),
                (@class, career) => new SingleClassDto
                {
                    Id = @class.Id,
                    ClassCode = @class.ClassCode,
                    CareerNames = career.Career.Name,
                    Careers = @class.ClassCareers.Select(x => x.CareerId)
                })
                .AsNoTracking()
                 .ToListAsync();

            return data.GroupBy(
                @class => @class.Id,
                (Id, @class) => new SingleClassDto
                {
                    Id = @class.First().Id,
                    ClassCode = @class.First().ClassCode,
                    CareerNames = string.Join(", ", @class.Select(c => c.CareerNames)),
                    ClassName = @class.First().ClassName,
                    Careers = @class.First().Careers
                }
                );
        }

        public async Task<Class> FindByCode(string code)
        {
            return await _classRepository.FirstOrDefault(c => c.ClassCode == code);
        }
        public async Task Create(CreateClassDto @class)
        {
            var careers = _careerRepository.Filter(career => @class.Careers.Contains(career.Id));

            var classInfo = new Class
            {
                ClassCode = @class.ClassCode,
                ClassName = @class.ClassName
            };

            foreach (var career in careers)
            {
                classInfo.ClassCareers.Add(new ClassCareer { Career = career });
            }

            await _classRepository.Add(classInfo);
        }

        public async Task<SingleClassDto> FindById(int id)
        {
            return await _classRepository
                .Filter(@class => !@class.Disabled && @class.Id == id)
                .Include(@class => @class.ClassCareers)
                    .ThenInclude(c => c.Career)
                .Select(@class => new SingleClassDto
                {
                    Id = @class.Id,
                    ClassCode = @class.ClassCode,
                    ClassName = @class.ClassName,
                    Careers = @class.ClassCareers.Select(x => x.CareerId)
                })
                .FirstOrDefaultAsync();
        }

        public async Task Remove(int id)
        {
            var @class = await _classRepository.FindById(id);
            await _classRepository.Disable(@class);
        }

        public async Task Update(int id, UpdateClassDto @class)
        {
            var subject = _classRepository
                .All()
                .Include(x => x.ClassCareers)
                .FirstOrDefault(x => x.Id == id);

            subject.ClassName = @class.ClassName;
            subject.ClassCode = @class.ClassCode;

            await _classRepository.Update(subject.ClassCareers, @class.Careers
                .Select(x => new ClassCareer
                {
                    CareerId = x,
                    ClassId = id
                }), x => x.CareerId);
        }

        public async Task<IEnumerable<ProjectsClassReportDto>> ProjectsByClass(string classCode)
        {
            return await _classRepository
                .Filter(x => x.ClassCode.Equals(classCode))

                .SelectMany(@class => @class.Sections,
                    (@class, section) => new
                    {
                        @class.ClassCode,
                        @class.ClassName,
                        ProfessorName = $"{section.Professor.FirstName} {section.Professor.FirstLastName}",
                        section.SectionProjects
                    })
                .SelectMany(@class => @class.SectionProjects, (subject, project) => new ProjectsClassReportDto
                {
                    ClassCode = subject.ClassCode,
                    ClassName = subject.ClassName,
                    ProfessorName = subject.ProfessorName,
                    ProjectCode = project.Project.Code,
                    ProjectName = project.Project.Name
                }).ToListAsync();


        }
    }
}
