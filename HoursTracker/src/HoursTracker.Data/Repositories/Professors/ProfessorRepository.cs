using HoursTracker.Domain;
using HoursTracker.Domain.Aggregates.Professors;
using Microsoft.EntityFrameworkCore;

namespace HoursTracker.Data.Repositories.Professors
{
    public class ProfessorRepository: EfRepository<Professor>, IProfessorRepository
    {
        public ProfessorRepository(DbContext context) : base(context)
        {
        }
    }
}