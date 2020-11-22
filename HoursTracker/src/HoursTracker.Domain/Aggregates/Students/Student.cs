using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;
using HoursTracker.Domain.Aggregates.Campuses;
using HoursTracker.Domain.Aggregates.Bot;
using HoursTracker.Domain.Contracts;
using HoursTracker.Domain.Shared;
using HoursTracker.Domain.Aggregates.ProjectHours;
using System.ComponentModel.DataAnnotations;

namespace HoursTracker.Domain.Aggregates.Students
{
    [Table("alumnos")]
    public class Student : BaseEntity, IAggregateRoot
    {
        [Column("codigo_alumno")]
        [StringLength(20)]
        public string Account { get; set; }

        [Column("primer_nombre")]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Column("segundo_nombre")]
        [StringLength(50)]
        public string SecondName { get; set; }

        [Column("primer_apellido")]
        [StringLength(50)]
        public string FirstSurname { get; set; }

        [Column("segundo_apellido")]
        [StringLength(50)]
        public string SecondSurname { get; set; }

        [Column("finiquito")]
        public bool Settlement { get; set; }

        [Column("correo_electronico")]
        [StringLength(50)]
        public string Email { get; set; }

        [Column("id_campus")]
        public int CampusId { get; set; }
        public Campus Campus { get; set; }

        public DataBot Data { get; set; }

        public ICollection<StudentCareer> StudentCareers { get; set; } = new HashSet<StudentCareer>();

        public ICollection<StudentSection> StudentSections { get; set; } = new HashSet<StudentSection>();

        public ICollection<ProjectHour> ProjectHours { get; set; } = new HashSet<ProjectHour>();
    }
}