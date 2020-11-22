using HoursTracker.Domain.Aggregates.Sections;
using HoursTracker.Domain.Aggregates.Students;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HoursTracker.Domain.Shared
{
    [Table("alumnos_x_secciones")]
    public class StudentSection
    {
        [Column("id_alumno")]
        public int StudentId { get; set; }

        [Column("id_seccion")]
        public int SectionId { get; set; }

        public Student Student { get; set; }

        public Section Section { get; set; }


    }
}
