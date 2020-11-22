using HoursTracker.Domain.Aggregates.Campuses;
using HoursTracker.Domain.Contracts;
using System.ComponentModel.DataAnnotations.Schema;

namespace HoursTracker.Domain.Aggregates.Professors
{
    [Table("docentes")]
    public class Professor : BaseEntity, IAggregateRoot
    {
        [Column("codigo_docente")]
        public string Code { get; set; }

        [Column("primer_nombre")]
        public string FirstName { get; set; }

        [Column("segundo_nombre")]
        public string SecondName { get; set; }

        [Column("primer_apellido")]
        public string  FirstLastName { get; set; }

        [Column("segundo_apellido")]
        public string SecondLastName { get; set; }

        [Column("id_campus")]
        public int? CampusId { get; set; }

        public Campus Campus { get; set; }

    }
}