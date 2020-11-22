using HoursTracker.Domain.Aggregates.Campuses;
using HoursTracker.Domain.Aggregates.Careers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HoursTracker.Domain.Shared
{
    [Table("campus_x_carreras")]
    public class CampusCareer
    {
        [Column("id_carrera")]
        public int CareerId { get; set; }

        [Column("id_campus")]
        public int CampusId { get; set; }

        public Campus Campus { get; set; }

        public Career Career { get; set; }
    }
}
