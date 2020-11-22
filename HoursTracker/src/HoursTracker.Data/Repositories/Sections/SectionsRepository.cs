using HoursTracker.Domain.Aggregates.Sections;
using Microsoft.EntityFrameworkCore;

namespace HoursTracker.Data.Repositories.Sections
{
    public class SectionsRepository : EfRepository<Section>, ISectionRepository
    {
        public SectionsRepository(DbContext context) : base(context)
        {

        }
    }
}
