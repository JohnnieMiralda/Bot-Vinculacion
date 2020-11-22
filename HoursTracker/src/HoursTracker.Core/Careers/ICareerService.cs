using HoursTracker.Domain.Aggregates.Careers;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HoursTracker.Core.Careers
{
    public interface ICareerService
    {
        Task<Career> FindByCode(string code);
        Task<Career> FindById(int id);

        Task Create(Career career);

        Task<IEnumerable<CareerDto>> All();

        Task Remove(int id);

        Task Update(int id, Career career);
    }
}
