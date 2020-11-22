using System;
using System.Collections.Generic;
using System.Text;

namespace HoursTracker.Core.Projects
{
    public class UpdateProjectDto
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public double Budget { get; set; }

        public int VinculationTypeId { get; set; }
    }
}
