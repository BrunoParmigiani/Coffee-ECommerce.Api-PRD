using Coffee_Ecommerce.API.Features.Administrator;
using Coffee_Ecommerce.API.Features.Announcement;
using Coffee_Ecommerce.API.Features.ApiAccess;
using Coffee_Ecommerce.API.Features.Establishment;
using Coffee_Ecommerce.API.Features.Favorite;
using Coffee_Ecommerce.API.Features.FooterItem;
using Coffee_Ecommerce.API.Features.NewOrder;
using Coffee_Ecommerce.API.Features.Order;
using Coffee_Ecommerce.API.Features.Product;
using Coffee_Ecommerce.API.Features.Report;
using Coffee_Ecommerce.API.Features.Resume;
using Coffee_Ecommerce.API.Features.User;
using Microsoft.EntityFrameworkCore;

namespace Coffee_Ecommerce.API.Infraestructure
{
    public class PostgreContext : DbContext
    {
        public PostgreContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<AdministratorEntity> Administrators { get; set; }
        public DbSet<AnnouncementEntity> Announcements { get; set; }
        public DbSet<ApiAccessEntity> ApiAccesses { get; set; }
        public DbSet<EstablishmentEntity> Establishments { get; set; }
        public DbSet<FavoriteEntity> Favorites { get; set; }
        public DbSet<FooterItemEntity> FooterItems { get; set; }
        public DbSet<NewOrderEntity> NewOrders { get; set; }
        public DbSet<OrderEntity> Orders { get; set; }
        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<ReportEntity> Reports { get; set; }
        public DbSet<ResumeEntity> Resumes { get; set; }
        public DbSet<UserEntity> Users { get; set; }
    }
}
