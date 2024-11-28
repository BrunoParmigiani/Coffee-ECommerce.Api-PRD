using Coffee_Ecommerce.API.Shared.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coffee_Ecommerce.API.Features.User
{
    [Table("Users")]
    public sealed class UserEntity : UserBase
    {
        [Column("CPF")]
        public string CPF { get; set; }

        [Column("Password")]
        public string Password { get; set; }

        [Column("PhoneNumber")]
        public string PhoneNumber { get; set; }

        [Column("Address")]
        public string Address { get; set; }

        [Column("Complement")]
        public string Complement { get; set; }

        [Column("Suspended")]
        public bool Suspended { get; set; } = false;

        [Column("DeleteSchedule")]
        public DateTime DeleteSchedule { get; set; } = DateTime.MaxValue.ToUniversalTime();
    }
}
