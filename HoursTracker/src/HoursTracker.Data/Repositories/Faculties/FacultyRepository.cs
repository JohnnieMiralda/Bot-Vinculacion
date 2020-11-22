using HoursTracker.Domain.Aggregates.Faculties;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HoursTracker.Data.Repositories.Faculties
{
    public class FacultyRepository : EfRepository<Faculty>, IFacultyRepository
    {
        public FacultyRepository(DbContext context) : base(context)
        {
        }
    }
}
