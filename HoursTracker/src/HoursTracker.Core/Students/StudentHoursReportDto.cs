using System;
using System.Collections.Generic;
using System.Text;

namespace HoursTracker.Core.Students
{
    public class StudentsHoursReportDto
    {
        public string Account { get; set; }

        public string StudentName { get; set; }

        public string ProjectCode { get; set; }

        public string ProjectName { get; set; }

        public string PeriodCode { get; set; }

        public string ClassName { get; set; }

        public string CareerCode { get; set; }

        public int HoursAmount { get; set; }
    }
}
