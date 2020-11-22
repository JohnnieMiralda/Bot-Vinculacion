using System;
using System.Collections.Generic;
using System.Text;

namespace HoursTracker.Core.Faculties
{
    public class HoursFacultiesReportDto
    {

        public string FacultyCode { get; set; }

        public string FacultyName { get; set; }

        public string ProjectCode { get; set; }

        public string ProjectName { get; set; }

        public string ClassCode { get; set; }

        public string ClassName { get; set; }

        public int HoursAmount { get; set; }
    }
}
