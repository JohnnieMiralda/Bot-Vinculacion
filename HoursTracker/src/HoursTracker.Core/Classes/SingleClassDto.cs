using System;
using System.Collections.Generic;
using System.Text;

namespace HoursTracker.Core.Classes
{
     public class SingleClassDto
    {
        public int Id { get; set; }

        public string ClassName { get; set; }

        public string ClassCode { get; set; }

        public string CareerNames { get; set; }

        public IEnumerable<int> Careers { get; set; }
    }
}
