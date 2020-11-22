using HoursTracker.Core.Students;
using HoursTracker.Domain.Aggregates.Sections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HoursTracker.Core.Sections
{
    public interface ISectionService
    {
        Task<Section> FindById(int id);
        Task<Section> FindByCode(string code);

        Task Create(CreateSectionDto section);

        Task<IEnumerable<SingleSectionDto>> All();

        Task Remove(int id);

        Task Update(int id, UpdateSectionDto section);

        Task<IEnumerable<SingleStudentDto>> FindStudentsBySection(int id);

    }
}
