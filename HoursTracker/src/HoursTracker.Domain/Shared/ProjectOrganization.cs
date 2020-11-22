using HoursTracker.Domain.Aggregates.ExternalOrganizations;
using HoursTracker.Domain.Aggregates.Projects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HoursTracker.Domain.Shared
{
    [Table("proyectos_x_organizaciones")]
    public class ProjectOrganization
    {
        [Column("id_proyecto")]
        public int ProjectId { get; set; }

        [Column("id_organizacion")]
        public int OrganizationId { get; set; }

        public Project Project { get; set; }

        public ExternalOrganization ExternalOrganization { get; set; }


    }
}
