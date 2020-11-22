using System;
using System.Collections.Generic;
using System.Text;

namespace HoursTracker.Core.Sections
{
    public class UpdateSectionDto
    {
        public string Code { get; set; }

        public int Period { get; set; }

        public int Class { get; set; }

        public int Professor { get; set; }

        public IEnumerable<string> Students { get; set; }
    }
}
