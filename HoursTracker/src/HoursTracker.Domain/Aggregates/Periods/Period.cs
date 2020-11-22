using HoursTracker.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HoursTracker.Domain.Aggregates.Periods
{
    [Table("periodos")]
    public class Period : BaseEntity, IAggregateRoot
    {
        [Column("codigo_periodo")]
        [StringLength(20)]
        public string Code { get; set; }

        [Column("anio_periodo")]
        [StringLength(10)]
        public string Year { get; set; }

        [Column("semestre_periodo")]
        [StringLength(10)]
        public string Semester { get; set; }

        [Column("trimestre_periodo")]
        [StringLength(10)]
        public string Trimester { get; set; }
    }
}
