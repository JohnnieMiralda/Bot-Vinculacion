using HoursTracker.Domain.Aggregates.Campuses;
using HoursTracker.Domain.Aggregates.Careers;
using HoursTracker.Domain.Aggregates.Classes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HoursTracker.Data.Repositories.Classes
{
    public class ClassRepository : EfRepository<Class>, IClassRepository
    {
        public ClassRepository(DbContext context) : base(context)
        {
        }

     
    }
}
