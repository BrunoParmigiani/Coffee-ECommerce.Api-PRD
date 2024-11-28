using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coffee_Ecommerce.API.Features.ApiAccess
{
    [Table("ApiAccess")]
    public sealed class ApiAccessEntity
    {
        [Key]
        [Column("Id")]
        public Guid Id { get; set; }

        [Column("ServiceName")]
        public string ServiceName { get; set; }

        [Column("Key")]
        public string? Key { get; set; }
    }
}
