using HoursTracker.Domain.Aggregates.Projects;
using HoursTracker.Domain.Aggregates.Sections;
using HoursTracker.Domain.Aggregates.Students;
using HoursTracker.Domain.Aggregates.VinculationTypes;
using HoursTracker.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HoursTracker.Domain.Aggregates.ProjectHours
{
    [Table("horas_proyectos")]
    public class ProjectHour : BaseEntity, IAggregateRoot
    {
        [Column("id_alumno")]
        public int StudentId { get; set; }
        public Student Student { get; set; }

        [Column("id_proyecto")]
        public int ProjectId { get; set; }
        public Project Project { get; set; }

        [Column("id_seccion")]
        public int SectionId { get; set; }
        public Section Section { get; set; }

        [Column("horas_trabajadas")]
        public int Hours { get; set; }
        [Column("estado")]
        public int TableState { get; set; }

    }
}
