using System.Collections.Generic;
using System.Threading.Tasks;
using HoursTracker.Domain.Aggregates.Professors;

namespace HoursTracker.Core.Professors
{
    public interface IProfessorService
    {
        Task<Professor> FindByCode(string code);
        Task<SingleProfessorDto> FindById(int id);
        
        Task Create(Professor professor);

        Task<IEnumerable<SingleProfessorDto>> All();

        Task Remove(int id);

        Task Update(int id, Professor professor);
    }
}