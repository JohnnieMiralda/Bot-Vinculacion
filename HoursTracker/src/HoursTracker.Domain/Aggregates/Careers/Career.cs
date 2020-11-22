using HoursTracker.Domain.Aggregates.Classes;
using HoursTracker.Domain.Aggregates.Faculties;
using HoursTracker.Domain.Contracts;
using HoursTracker.Domain.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HoursTracker.Domain.Aggregates.Careers
{
    [Table("carreras")]
    public class Career : BaseEntity, IAggregateRoot
    {
        [Column("codigo_carrera")]
        [StringLength(20)]
        public string Code { get; set; }

        [Column("nombre_carrera")]
        [StringLength(50)]
        public string Name { get; set; }

        public ICollection<ClassCareer> ClassCareers { get; set; } = new HashSet<ClassCareer>();

        public ICollection<StudentCareer> StudentCareers { get; set; } = new HashSet<StudentCareer>();

        public ICollection<CampusCareer> CampusCareer { get; set; } = new HashSet<CampusCareer>();

        [Column("id_facultad")]
        public int FacultyId { get; set; }
        public Faculty Faculty { get; set; }
    }
}
