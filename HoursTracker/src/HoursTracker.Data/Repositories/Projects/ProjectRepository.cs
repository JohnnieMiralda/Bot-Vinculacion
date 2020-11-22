using HoursTracker.Domain.Aggregates.Projects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HoursTracker.Data.Repositories.Projects
{
    public class ProjectRepository : EfRepository<Project>, IProjectRepository
    {
        public ProjectRepository(DbContext context) : base(context)
        {
        }
    }
}
