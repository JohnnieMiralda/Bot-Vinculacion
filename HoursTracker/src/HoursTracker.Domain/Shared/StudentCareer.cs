using HoursTracker.Domain.Aggregates.Careers;
using HoursTracker.Domain.Aggregates.Students;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HoursTracker.Domain.Shared
{
    [Table("alumnos_x_carreras")]
    public class StudentCareer
    {
        [Column("id_carrera")]
        public int CareerId { get; set; }

        [Column("id_alumno")]
        public int StudentId { get; set; }

        public Student Student  { get; set; }

        public Career Career { get; set; }
    }
}
