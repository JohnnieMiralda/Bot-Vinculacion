using System;
using System.Collections.Generic;
using System.Text;

namespace HoursTracker.Core.Classes
{
    public class UpdateClassDto
    {
        public string ClassName { get; set; }

        public string ClassCode { get; set; }

        public IEnumerable<int> Careers { get; set; }
    }
}
