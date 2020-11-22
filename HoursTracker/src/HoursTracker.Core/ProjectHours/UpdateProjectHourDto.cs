using System;
using System.Collections.Generic;
using System.Text;

namespace HoursTracker.Core.ProjectHours
{
    public class UpdateProjectHourDto
    {
        public int Hours { get; set; }
        public int TableState { get; set; }
        public string Student { get; set; }
        public int Section { get; set; }
        public int Project { get; set; }
    }
}
