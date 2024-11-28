using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coffee_Ecommerce.API.Features.Announcement
{
    [Table("Announcements")]
    public sealed class AnnouncementEntity
    {
        [Key]
        [Column("Id")]
        public Guid Id { get; set; }

        [Column("CreatorId")]
        public Guid CreatorId { get; set; }

        [Column("Name")]
        public string Name { get; set; }

        [Column("Message")]
        public string Message { get; set; }

        [Column("Recipients")]
        public string Recipients { get; set; }
    }
}
