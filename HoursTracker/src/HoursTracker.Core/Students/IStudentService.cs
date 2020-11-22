using System.Collections.Generic;
using System.Threading.Tasks;
using HoursTracker.Domain.Aggregates.Students;

namespace HoursTracker.Core.Students
{
    public interface IStudentService
    {
        Task<Student> FindByCode(string code);
        Task<SingleStudentDto> FindById(int id);

        Task Create(CreateStudentDto student);

        Task<IEnumerable<SingleStudentDto>> All();

        Task Remove(int id);

        Task Update(int id, UpdateStudentDto student);

        Task<IEnumerable<StudentsHoursReportDto>> HoursByStudent(string account);
        Task<SingleStudentDto> FindByAccount(string id);
    }
}