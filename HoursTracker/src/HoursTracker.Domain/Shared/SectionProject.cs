using HoursTracker.Domain.Aggregates.Projects;
using HoursTracker.Domain.Aggregates.Sections;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HoursTracker.Domain.Shared
{
    [Table("secciones_x_proyectos")]
    public class SectionProject
    {
        [Column("id_seccion")]
        public int SectionId { get; set; }

        [Column("id_proyecto")]
        public int ProjectId { get; set; }

        public Section Section { get; set; }

        public Project Project { get; set; }
    }
}
