using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoursTracker.Web.Models
{
    public class SectionViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; }

        public string Period { get; set; }

        public string Class { get; set; }

        public string Professor { get; set; }
    }
}
