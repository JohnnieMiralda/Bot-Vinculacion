



using HoursTracker.Domain.Aggregates.Professors;
using HoursTracker.Domain.Contracts;
using HoursTracker.Domain.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HoursTracker.Domain.Aggregates.Campuses
{
    [Table("campuses")]
    public class Campus : BaseEntity, IAggregateRoot
    {

        [Column("codigo_campus")]
        [StringLength(20)]
        public string Code { get; set; }


        [Column("nombre_campus")]
        [StringLength(50)]
        public string Name { get; set; }

        public ICollection<CampusCareer> CampusCareer { get; set; } = new HashSet<CampusCareer>();

        public ICollection<Professor> Professor { get; set; } = new HashSet<Professor>();
    }
}
