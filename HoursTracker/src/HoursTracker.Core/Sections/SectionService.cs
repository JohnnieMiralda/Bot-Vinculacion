using HoursTracker.Core.Students;
using HoursTracker.Domain.Aggregates.Classes;
using HoursTracker.Domain.Aggregates.Periods;
using HoursTracker.Domain.Aggregates.Professors;
using HoursTracker.Domain.Aggregates.Sections;
using HoursTracker.Domain.Aggregates.Students;
using HoursTracker.Domain.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoursTracker.Core.Sections
{
    public class SectionService : ISectionService
    {
        private readonly ISectionRepository _sectionRepository;
        private readonly IProfessorRepository _professorRepository;
        private readonly IPeriodRepository _periodRepository;
        private readonly IClassRepository _classRepository;
        private readonly IStudentRepository _studentRepository;

        public SectionService(
            ISectionRepository sectionRepository,
            IProfessorRepository professorRepository,
            IPeriodRepository periodRepository,
            IClassRepository classRepository,
            IStudentRepository studentRepository)
        {
            _sectionRepository = sectionRepository;
            _professorRepository = professorRepository;
            _periodRepository = periodRepository;
            _classRepository = classRepository;
            _studentRepository = studentRepository;
        }

        public async Task<Section> FindById(int id)
        {
            return await _sectionRepository.FindById(id);
        }

        public async Task<IEnumerable<SingleSectionDto>> All()
        {
            return await _sectionRepository
                .Filter(section => !section.Disabled)
                .Select(s => new SingleSectionDto
                {
                    Id = s.Id,
                    Class = s.Class.ClassName,
                    Period = s.Period.Code,
                    Professor = s.Professor.FirstName + " " + s.Professor.SecondName + " " + s.Professor.FirstLastName + " " + s.Professor.SecondLastName,
                    Code = s.Code
                }).ToListAsync();
        }

        public async Task Remove(int id)
        {
            var section = await _sectionRepository.FindById(id);
            await _sectionRepository.Disable(section);
        }

        public async Task Update(int id, UpdateSectionDto section)
        {
            var sec = await _sectionRepository.FindById(id);
            var prof = await _professorRepository.FindById(section.Professor);
            var per = await _periodRepository.FindById(section.Period);
            var cla = await _classRepository.FindById(section.Class);

            var studentIds = _studentRepository.Filter(x => section.Students.Contains(x.Account)).Select(x => x.Id);

            sec.Professor = prof;
            sec.Period = per;
            sec.Class = cla;
            sec.Code = section.Code;

            await _sectionRepository.Update(sec.StudentSections, studentIds.Select(x => new StudentSection
            {
                StudentId = x,
                SectionId = id
            }), x => x.StudentId);
        }

        public async Task Create(CreateSectionDto section)
        {
            var clase = await _classRepository.FindById(section.Class);
            var period = await _periodRepository.FindById(section.Period);
            var professor = await _professorRepository.FindById(section.Professor);

            var sectionInfo = new Section
            {
                Code = section.Code,
                Professor = professor,
                Period = period,
                Class = clase,
            };

            if (section.Students != null && section.Students.Count() > 0)
            {
                var students = _studentRepository.Filter(student => section.Students.Contains(student.Account)).ToList();

                foreach (var student in students)
                {
                    sectionInfo.StudentSections.Add(new StudentSection { Student = student });
                }
            }

            await _sectionRepository.Add(sectionInfo);
        }

        public async Task<Section> FindByCode(string code)
        {
            return await _sectionRepository.FirstOrDefault(c => c.Code == code);
        }

        public async Task<IEnumerable<SingleStudentDto>> FindStudentsBySection(int id)
        {
            var data = await _studentRepository.Filter(x => x.StudentSections.Any(x => x.SectionId == id)).Select(x => new SingleStudentDto
            {
                Id = x.Id,
                Account = x.Account,
                Email = x.Email,
                FirstName = x.FirstName,
                SecondName = x.SecondName,
                FirstSurname = x.FirstSurname,
                SecondSurname = x.SecondSurname,
            }).ToListAsync();

            return data;
        }
    }
}
