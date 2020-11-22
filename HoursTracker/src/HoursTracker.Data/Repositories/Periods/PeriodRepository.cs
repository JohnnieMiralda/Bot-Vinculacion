using HoursTracker.Domain.Aggregates.Periods;
using Microsoft.EntityFrameworkCore;

namespace HoursTracker.Data.Repositories.Periods
{
    public class PeriodRepository : EfRepository<Period>, IPeriodRepository
    {
        public PeriodRepository(DbContext context) : base(context)
        {

        }
    }
}
