using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coffee_Ecommerce.API.Features.FooterItem
{
    [Table("FooterItems")]
    public sealed class FooterItemEntity
    {
        [Key]
        [Column("Id")]
        public Guid Id { get; set; }

        [Column("Name")]
        public string Name { get; set; }

        [Column("Link")]
        public string Link { get; set; }
    }
}
