using HoursTracker.Domain.Aggregates.Campuses;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HoursTracker.Core.Campuses
{
    public interface ICampusService
    {
        Task<Campus> FindByCode(string code);

        Task<Campus> FindById(int id);

        Task Create(Campus campus);

        Task<IEnumerable<Campus>> All();

        Task Remove(int id);

        Task Update(int id, Campus campus);
    }
}
