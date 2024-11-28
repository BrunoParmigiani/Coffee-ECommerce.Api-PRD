using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Coffee_Ecommerce.API.Shared.Models
{
    public abstract class UserBase
    {
        [Key]
        [Column("Id")]
        public Guid Id { get; set; }

        [Column("Name")]
        public string Name { get; set; }

        [Column("Email")]
        public string Email { get; set; }

        [Column("RefreshToken")]
        public string? RefreshToken { get; set; }

        [Column("RefreshTokenExpireTime")]
        public DateTime RefreshTokenExpireTime { get; set; } = default;

        [Column("Role")]
        public string Role { get; set; }
    }
}
