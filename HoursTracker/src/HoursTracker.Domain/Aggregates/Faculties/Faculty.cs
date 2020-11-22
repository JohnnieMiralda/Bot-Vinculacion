using HoursTracker.Domain.Aggregates.Careers;
using HoursTracker.Domain.Aggregates.ProjectHours;
using HoursTracker.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HoursTracker.Domain.Aggregates.Faculties
{
    [Table("facultades")]
    public class Faculty : BaseEntity, IAggregateRoot
    {
        [Column("codigo_facultad")]
        [StringLength(20)]
        public string Code { get; set; }

        [Column("nombre_facultad")]
        [StringLength(50)]
        public string Name { get; set; }

        public ICollection<Career> Careers { get; set; } = new HashSet<Career>();
    }
}
