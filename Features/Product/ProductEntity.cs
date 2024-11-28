using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coffee_Ecommerce.API.Features.Product
{
    [Table("Products")]
    public sealed class ProductEntity
    {
        [Key]
        [Column("Id")]
        public Guid Id { get; set; }

        [Column("CreatorId")]
        public Guid CreatorId { get; set; }

        [Column("Name")]
        public string Name { get; set; }

        [Column("Price")]
        public float Price { get; set; }

        [Column("Description")]
        public string Description { get; set; }

        [Column("EstablishmentId")]
        public Guid EstablishmentId { get; set; }
    }
}
