using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoursTracker.Web.Models
{
    public class ClassViewModel
    {
        public int Id { get; set; }

        public string ClassName { get; set; }

        public string ClassCode { get; set; }


        public string CareerNames { get; set; }
        public IEnumerable<int> Careers { get; set; }
    }
}
