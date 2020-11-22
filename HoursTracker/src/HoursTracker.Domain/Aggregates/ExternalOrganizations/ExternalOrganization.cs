using HoursTracker.Domain.Aggregates.Projects;
using HoursTracker.Domain.Contracts;
using HoursTracker.Domain.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HoursTracker.Domain.Aggregates.ExternalOrganizations
{
    [Table("organizaciones_externas")]
    public class ExternalOrganization : BaseEntity, IAggregateRoot
    {
        [Column("codigo_organizacion")]
        [StringLength(20)]
        public string Code { get; set; }

        [Column("nombre_organizacion")]
        [StringLength(50)]
        public string Name { get; set; }

        [Column("direccion_organizacion")]
        [StringLength(50)]
        public string Address { get; set; }

        [Column("contacto_organizacion")]
        [StringLength(30)]
        public string Contact { get; set; }

        public ICollection<ProjectOrganization> ProjectOrganizations { get; set; } = new HashSet<ProjectOrganization>();
    }
}
