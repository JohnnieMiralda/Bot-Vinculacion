using HoursTracker.Domain.Aggregates.ProjectHours;
using HoursTracker.Domain.Aggregates.VinculationTypes;
using HoursTracker.Domain.Contracts;
using HoursTracker.Domain.Shared;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HoursTracker.Domain.Aggregates.Projects
{
    [Table("proyectos")]
    public class Project : BaseEntity, IAggregateRoot
    {
        [Column("codigo_proyecto")]
        [StringLength(20)]
        public string Code { get; set; }

        [Column("nombre_proyecto")]
        [StringLength(250)]
        public string Name { get; set; }

        [Column("costo_proyecto")]
        public double Budget { get; set; }

        [Column("id_vinculacion")]
        public int VinculationTypeId { get; set; }

        public VinculationType VinculationType { get; set; }

        public ICollection<SectionProject> SectionProjects { get; set; } = new HashSet<SectionProject>();

        public ICollection<ProjectOrganization> ProjectOrganizations { get; set; } = new HashSet<ProjectOrganization>();

        public ICollection<ProjectHour> ProjectHours { get; set; } = new HashSet<ProjectHour>();
    }
}
