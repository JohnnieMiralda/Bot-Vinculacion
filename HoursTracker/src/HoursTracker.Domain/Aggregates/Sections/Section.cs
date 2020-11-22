using HoursTracker.Domain.Aggregates.Classes;
using HoursTracker.Domain.Aggregates.Periods;
using HoursTracker.Domain.Aggregates.Professors;
using HoursTracker.Domain.Aggregates.ProjectHours;
using HoursTracker.Domain.Contracts;
using HoursTracker.Domain.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HoursTracker.Domain.Aggregates.Sections
{
    [Table("secciones")]
    public class Section : BaseEntity, IAggregateRoot
    {
        
        [Column("codigo_seccion")]
        [StringLength(20)]
        public string Code { get; set; }

        [Column("id_periodo")]
        public int PeriodId { get; set; }
        public Period Period { get; set; }

        [Column("id_asignatura")]
        public int ClassId { get; set; }
        public Class Class { get; set; }

        [Column("id_docente")]
        public int ProfessorId { get; set; }
        public Professor Professor { get; set; }

        public ICollection<StudentSection> StudentSections { get; set; } = new HashSet<StudentSection>();

        public ICollection<SectionProject> SectionProjects { get; set; } = new HashSet<SectionProject>();

        public ICollection<ProjectHour> ProjectHours { get; set; } = new HashSet<ProjectHour>();
    }

    
}
