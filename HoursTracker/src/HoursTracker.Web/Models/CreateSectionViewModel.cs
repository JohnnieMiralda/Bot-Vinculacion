using System.Collections;
using System.Collections.Generic;

namespace HoursTracker.Web.Models
{
    public class CreateSectionViewModel
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public int Period { get; set; }

        public int Class { get; set; }

        public int Professor { get; set; }

        public IEnumerable<string> Students { get; set; }
    }
}