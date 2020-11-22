using HoursTracker.Domain.Aggregates.Campuses;
using HoursTracker.Domain.Aggregates.Careers;
using HoursTracker.Domain.Aggregates.Students;
using HoursTracker.Domain.Shared;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace HoursTracker.Core.Students
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly ICareerRepository _careerRepository;
        private readonly ICampusRepository _campusRepository;
        private readonly IDataBotRepository _dataBotRepository;

        public StudentService(
            IStudentRepository studentRepository,
            ICareerRepository careerRepository,
            ICampusRepository campusRepository,
            IDataBotRepository dataBotRepository)
        {
            _studentRepository = studentRepository;
            _careerRepository = careerRepository;
            _campusRepository = campusRepository;
            _dataBotRepository = dataBotRepository;
        }

        public async Task<SingleStudentDto> FindById(int id)
        {
            var student = _studentRepository
                .Filter(x => x.Id == id)
                .Select(student => new SingleStudentDto
                {
                    Account = student.Account,
                    Email = student.Email,
                    Campus = student.CampusId,
                    FirstName = student.FirstName,
                    SecondName = student.SecondName,
                    FirstSurname = student.FirstSurname,
                    SecondSurname = student.SecondSurname,
                    Careers = student.StudentCareers.Select(x => x.CareerId),
                    Settlement = student.Settlement,
                    Id = student.Id

                }).FirstOrDefault();

            return student;
        }

        public async Task<IEnumerable<SingleStudentDto>> All()
        {
            var data = await _studentRepository.
                Filter(student => !student.Disabled)
                .Include(students => students.StudentCareers)

                .ThenInclude(c => c.Career)
                .Include(students => students.Data)
                .SelectMany(student => student.StudentCareers,
                    (student, career) => new SingleStudentDto
                    {
                        Id = student.Id,
                        Account = student.Account,
                        CampusName = student.Campus.Name,
                        CareerName = career.Career.Name,
                        FirstName = student.FirstName,
                        FirstSurname = student.FirstSurname,
                        SecondName = student.SecondName,
                        SecondSurname = student.SecondSurname,
                        Settlement = student.Settlement,
                        Email = student.Email,
                        isInBot = student.Data.Verified == 1,
                        TelegramAccount = student.Data.Telegramid
                    })
                    .ToListAsync();

            return data.GroupBy(
               student => student.Id,
               (id, student) => new SingleStudentDto
               {
                   Id = student.First().Id,
                   Account = student.First().Account,
                   CampusName = string.Join(", ", student.Select(s => s.CampusName)),
                   CareerName = string.Join(", ", student.Select(s => s.CareerName)),
                   FirstName = student.First().FirstName,
                   FirstSurname = student.First().FirstSurname,
                   SecondName = student.First().SecondName,
                   SecondSurname = student.First().SecondSurname,
                   Settlement = student.First().Settlement,
                   Email = student.First().Email,
                   isInBot = student.First().isInBot,
                   TelegramAccount = student.First().TelegramAccount
               }
            );
        }

        public async Task Remove(int id)
        {
            var student = await _studentRepository.FindById(id);
            await _studentRepository.Disable(student);
        }

        public async Task Update(int id, UpdateStudentDto student)
        {
            var stud = _studentRepository
                .All()
                .Include(x => x.StudentCareers)
                .FirstOrDefault(x => x.Id == id);

            if (stud.Email != student.Email)
            {
                var studentBot = await _dataBotRepository.FirstOrDefault(x => x.Student.Id == id);
                if(studentBot != null)
                {
                    await _dataBotRepository.Delete(studentBot);
                }
            }
            stud.Account = student.Account;
            stud.FirstName = student.FirstName;
            stud.SecondName = student.SecondName;
            stud.FirstSurname = student.FirstSurname;
            stud.SecondSurname = student.SecondSurname;
            stud.Settlement = student.Settlement;
            stud.Email = student.Email;

            await _studentRepository.Update(stud.StudentCareers, @student.Careers
                    .Select(x => new StudentCareer()
                    {
                        CareerId = x,
                        StudentId = id
                    }), x => x.CareerId);
        }

        public async Task<Student> FindByCode(string code)
        {
            return await _studentRepository.FirstOrDefault(c => c.Account == code);
        }
        public async Task Create(CreateStudentDto student)
        {
            var careers = _careerRepository.Filter(career => student.Careers.Contains(career.Id));
            var campus = await _campusRepository.FindById(student.Campus);

            var studentInfo = new Student
            {
                Account = student.Account,
                FirstName = student.FirstName,
                SecondName = student.SecondName,
                FirstSurname = student.FirstSurname,
                SecondSurname = student.SecondSurname,
                Campus = campus,
                Settlement = student.Settlement,
                Email = student.Email
            };

            foreach (var career in careers)
            {
                studentInfo.StudentCareers.Add(new StudentCareer { Career = career });
            }
            var stud = _studentRepository
                .All()
                .Include(x => x.StudentCareers)
                .FirstOrDefault(x => x.Account == studentInfo.Account);

            await _studentRepository.Add(studentInfo);





        }

        public async Task<IEnumerable<StudentsHoursReportDto>> HoursByStudent(string account)
        {            
            return await _studentRepository
                .Filter(x => x.Account.Equals(account))
                .Include(x => x.ProjectHours)
                .ThenInclude(s => s.Section)
                .ThenInclude(x => x.Class)
                .Include(x => x.ProjectHours)
                .ThenInclude(s => s.Section)
                .ThenInclude(x => x.Period)
                .Include(x => x.ProjectHours)
                .ThenInclude(x => x.Project)
                .SelectMany(student => student.ProjectHours.DefaultIfEmpty(),
                    (student, hours) => new StudentsHoursReportDto
                    {
                        Account = student.Account,
                        ClassName = hours.Section.Class.ClassName,
                        HoursAmount = hours.Hours,
                        ProjectName = hours.Project.Name,
                        PeriodCode = hours.Section.Period.Code,
                        StudentName = $"{student.FirstName} {student.FirstSurname}",
                        ProjectCode = hours.Project.Code,
                        CareerCode = student.StudentCareers.FirstOrDefault().Career.Name
                    }).ToListAsync();
        }

        public async Task<SingleStudentDto> FindByAccount(string id)
        {
            var student = await _studentRepository.FirstOrDefault(x => x.Account.Equals(id));


            if (student == null)
            {
                throw new System.Exception();
            }

            return new SingleStudentDto
            {
                Account = student.Account,
                Email = student.Email,
                Id = student.Id,
                FirstName = student.FirstName,
                FirstSurname = student.FirstSurname,
                SecondName = student.SecondName,
                SecondSurname = student.SecondSurname,
            };
        }
    }
}