using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coffee_Ecommerce.API.Features.Report
{
    [Table("Reports")]
    public sealed class ReportEntity
    {
        [Key]
        [Column("Id")]
        public Guid Id { get; set; }

        [Column("UserId")]
        public Guid UserId { get; set; }

        [Column("EstablishmentId")]
        public Guid EstablishmentId { get; set; }

        [Column("OrderId")]
        public Guid OrderId { get; set; }

        [Column("OpenTime")]
        public DateTime OpenTime { get; set; }

        [Column("CloseTime")]
        public DateTime CloseTime { get; set; }

        [Column("Problem")]
        public int Problem { get; set; }

        [Column("Description")]
        public string Description { get; set; }

        [Column("Status")]
        public int Status { get; set; }
    }
}
