using HoursTracker.Domain.Aggregates.Students;
using Microsoft.EntityFrameworkCore;

namespace HoursTracker.Data.Repositories.Students
{
    public class StudentsRepository : EfRepository<Student>, IStudentRepository
    {
        public StudentsRepository(DbContext context) : base(context)
        {

        }
    }
}