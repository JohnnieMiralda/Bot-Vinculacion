using System.Collections.Generic;
using System.Threading.Tasks;
using HoursTracker.Domain.Aggregates.VinculationTypes;

namespace HoursTracker.Core.VinculationTypes
{
    public interface IVinculationTypeService 
    {
        Task<VinculationType> FindByCode(string code);

        Task<VinculationType> FindById(int id);

        Task Create(VinculationType vType);

        Task<IEnumerable<VinculationType>> All();

        Task Remove(int id);

        Task Update(int id, VinculationType vType);
    }
}
