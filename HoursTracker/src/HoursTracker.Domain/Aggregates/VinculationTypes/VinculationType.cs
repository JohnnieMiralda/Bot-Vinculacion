using HoursTracker.Domain.Aggregates.Projects;
using HoursTracker.Domain.Contracts;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HoursTracker.Domain.Aggregates.VinculationTypes
{
    [Table("tipos_vinculacion")]
    public class VinculationType :  BaseEntity, IAggregateRoot
    {
        [Column("codigo_vinculacion")]
        [StringLength(20)]
        public string Code { get; set; }

        [Column("tipo_vinculacion")]
        [StringLength(50)]
        public string Type { get; set; }

        public ICollection<Project> Project { get; set; } = new HashSet<Project>();
    }
}
