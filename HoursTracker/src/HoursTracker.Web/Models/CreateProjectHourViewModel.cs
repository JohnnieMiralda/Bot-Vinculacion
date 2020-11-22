using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoursTracker.Web.Models
{
    public class CreateProjectHourViewModel
    {
        public int Hours { get; set; }
        public int TableState { get; set; }
        public string Student { get; set; }
        public int Section { get; set; }
        public int Project { get; set; }
    }
}
