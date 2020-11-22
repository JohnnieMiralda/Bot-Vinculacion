using HoursTracker.Domain.Aggregates.Students;
using HoursTracker.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HoursTracker.Domain.Aggregates.Bot
{
    [Table("alumnos_bot")]
    public class DataBot : BaseEntity, IAggregateRoot
    {
        [Column("cuenta_telegram")]
        [StringLength(50)]
        public string Telegramid { get; set; }
        [Column("token_generado")]
        [StringLength(20)]
        public string Token { get; set; }
        [Column("confirmado")]
        public int Verified { get; set; }
        [Column("fecha_confirmacion")]
        public DateTime Fecha_confirmacion { get; set; }
        [Column("estado")]
        public int Estado { get; set; }
        [Column("fecha_ultimo_token")]
        public DateTime Fecha_UltimoToken { get; set; }
        [Column("id_alumno")]
        public int StudentRef { get; set; }
        public Student Student { get; set; }
    }
}
