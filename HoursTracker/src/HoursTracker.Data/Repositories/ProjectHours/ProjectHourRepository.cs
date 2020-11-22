using HoursTracker.Domain.Aggregates.ProjectHours;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HoursTracker.Data.Repositories.ProjectHours
{
    public class ProjectHourRepository : EfRepository<ProjectHour>, IProjectHourRepository
    {
        public ProjectHourRepository(DbContext context) : base(context)
        {
        }
    }
}
