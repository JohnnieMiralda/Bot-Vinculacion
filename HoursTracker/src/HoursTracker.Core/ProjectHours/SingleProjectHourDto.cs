using System;
using System.Collections.Generic;
using System.Text;

namespace HoursTracker.Core.ProjectHours
{
    public class SingleProjectHourDto
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
