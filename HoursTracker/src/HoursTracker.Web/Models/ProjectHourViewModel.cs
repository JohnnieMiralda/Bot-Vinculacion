using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoursTracker.Web.Models
{
    public class ProjectHourViewModel
    {
        public int Id { get; set; }
        public int Hours { get; set; }
        public int TableState { get; set; }
        public string StudentAccount { get; set; }
        public string StudentFirstName { get; set; }
        public string StudentLastName { get; set; }
        public string ProjectName { get; set; }
        public string SectionCode { get; set; }
    }
}
