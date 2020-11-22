using HoursTracker.Domain.Aggregates.Classes;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HoursTracker.Core.Classes
{
    public interface IClassService
    {

        Task<Class> FindByCode(string code);
        Task<SingleClassDto> FindById(int id);

        Task Create(CreateClassDto @class);

        Task<IEnumerable<SingleClassDto>> All();

        Task<IEnumerable<SingleClassDto>> SingleClasses();

        Task Remove(int id);

        Task Update(int id, UpdateClassDto @class);

        Task<IEnumerable<ProjectsClassReportDto>> ProjectsByClass(string classCode);

    }
}
