using HoursTracker.Domain.Aggregates.Periods;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HoursTracker.Core.Periods
{
    public interface IPeriodService
    {
        Task<Period> FindById(int id);

        Task Create(Period period);

        Task<IEnumerable<Period>> All();

        Task Remove(int id);

        Task Update(int id, Period period);
    }
}
