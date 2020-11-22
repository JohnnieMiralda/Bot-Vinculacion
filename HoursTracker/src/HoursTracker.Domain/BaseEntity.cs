using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HoursTracker.Domain
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Column("deshabilitado")]

        public bool Disabled { get; set; }

    }
}