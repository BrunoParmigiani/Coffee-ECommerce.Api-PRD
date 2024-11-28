using Coffee_Ecommerce.API.Shared.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coffee_Ecommerce.API.Features.Establishment
{
    [Table("Establishments")]
    public sealed class EstablishmentEntity : UserBase
    {
        [Column("Address")]
        public string Address { get; set; }

        [Column("Complement")]
        public string Complement { get; set; }

        [Column("CNPJ")]
        public string CNPJ { get; set; }

        [Column("AdministratorId")]
        public Guid AdministratorId { get; set; }

        [Column("PhoneNumber")]
        public string PhoneNumber { get; set; }

        [Column("Password")]
        public string Password { get; set; }

        [Column("Blocked")]
        public bool Blocked { get; set; } = false;

        [Column("DeleteSchedule")]
        public DateTime DeleteSchedule { get; set; } = DateTime.MaxValue.ToUniversalTime();
    }
}
