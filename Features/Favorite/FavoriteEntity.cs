using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coffee_Ecommerce.API.Features.Favorite
{
    [Table("Favorites")]
    public sealed class FavoriteEntity
    {
        [Key]
        [Column("Id")]
        public Guid Id { get; set; }

        [Column("UserId")]
        public Guid UserId { get; set; }

        [Column("ProductId")]
        public Guid ProductId { get; set; }
    }
}
