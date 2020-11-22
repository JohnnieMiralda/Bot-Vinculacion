using HoursTracker.Domain.Aggregates.Careers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HoursTracker.Data.Repositories.Careers
{
    public class CareerRepository : EfRepository<Career>, ICareerRepository
    {
        public CareerRepository(DbContext context) : base(context)
        {
        } 
    }
}
