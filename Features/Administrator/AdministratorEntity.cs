using Coffee_Ecommerce.API.Shared.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coffee_Ecommerce.API.Features.Administrator
{
    [Table("Administrators")]
    public sealed class AdministratorEntity : UserBase
    {
        [Column("CPF")]
        public string CPF { get; set; }

        [Column("Password")]
        public string Password { get; set; }

        [Column("PhoneNumber")]
        public string PhoneNumber { get; set; }

        [Column("Blocked")]
        public bool Blocked { get; set; } = false;

        [Column("DeleteSchedule")]
        public DateTime DeleteSchedule { get; set; } = DateTime.MaxValue.ToUniversalTime();

        [Column("EstablishmentId")]
        public Guid EstablishmentId { get; set; }
    }
}
