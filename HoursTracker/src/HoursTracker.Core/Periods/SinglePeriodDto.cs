using System;
using System.Collections.Generic;
using System.Text;

namespace HoursTracker.Core.Periods
{
    public class SinglePeriodDto
    {
        public int Id { get; set; }

        public string PeriodCode { get; set; }

        public string PeriodYear { get; set; }

        public string PeriodSemester { get; set; }

        public string PeriodTrimester { get; set; }
    }
}
