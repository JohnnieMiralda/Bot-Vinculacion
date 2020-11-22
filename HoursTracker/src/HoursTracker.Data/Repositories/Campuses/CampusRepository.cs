using HoursTracker.Domain.Aggregates.Campuses;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HoursTracker.Data.Repositories.Campuses
{
    public class CampusRepository : EfRepository<Campus>, ICampusRepository
    {
        public CampusRepository(DbContext context) : base(context)
        {
        }
    }
}
