using HoursTracker.Domain.Aggregates.Sections;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HoursTracker.Data.Repositories.Sections
{
    public class SectionRepository : EfRepository<Section>, ISectionRepository
    {
        public SectionRepository(DbContext context) : base(context)
        {
        }
    }
}
