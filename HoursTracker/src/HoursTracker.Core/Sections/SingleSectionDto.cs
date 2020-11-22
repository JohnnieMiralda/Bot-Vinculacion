using System;
using System.Collections.Generic;
using System.Text;

namespace HoursTracker.Core.Sections
{
    public class SingleSectionDto
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string Period { get; set; }

        public string Class { get; set; }

        public string Professor { get; set; }
    }
}
