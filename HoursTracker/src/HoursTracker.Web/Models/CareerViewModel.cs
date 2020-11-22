using HoursTracker.Domain.Aggregates.Faculties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoursTracker.Web.Models
{
    public class CareerViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public int FacultyId { get; set; }
        public string FacultyName { get; set; }
    }
}
