using HoursTracker.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace HoursTracker.Domain.Aggregates.Periods
{
    public interface IPeriodRepository : IBaseRepository<Period>
    {
    }
}
