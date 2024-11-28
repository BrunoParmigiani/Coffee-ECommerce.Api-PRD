using Coffee_Ecommerce.API.Features.Administrator.Business;
using Coffee_Ecommerce.API.Features.Administrator.Repository;
using Coffee_Ecommerce.API.Features.Announcement.Repository;
using Coffee_Ecommerce.API.Features.Announcement.Business;
using Coffee_Ecommerce.API.Features.Establishment.Business;
using Coffee_Ecommerce.API.Features.Establishment.Repository;
using Coffee_Ecommerce.API.Features.FooterItem.Repository;
using Coffee_Ecommerce.API.Features.FooterItem.Business;
using Coffee_Ecommerce.API.Features.Order.Business;
using Coffee_Ecommerce.API.Features.Order.Repository;
using Coffee_Ecommerce.API.Features.Product.Business;
using Coffee_Ecommerce.API.Features.Product.Repository;
using Coffee_Ecommerce.API.Features.Report.Business;
using Coffee_Ecommerce.API.Features.Report.Repository;
using Coffee_Ecommerce.API.Features.Resume.Business;
using Coffee_Ecommerce.API.Features.User.Business;
using Coffee_Ecommerce.API.Features.User.Repository;
using Coffee_Ecommerce.API.Features.Authentication.Business;
using Coffee_Ecommerce.API.Services;
using Coffee_Ecommerce.API.Features.Resume.S3;
using Coffee_Ecommerce.API.Services.Interfaces;
using Coffee_Ecommerce.API.Features.Favorite.Business;
using Coffee_Ecommerce.API.Features.Favorite.Repository;
using Coffee_Ecommerce.API.Features.ApiAccess.Business;
using Coffee_Ecommerce.API.Features.ApiAccess.Repository;
using Coffee_Ecommerce.API.Features.NewOrder.Repository;
using Coffee_Ecommerce.API.Features.NewOrder.Business;

namespace Coffee_Ecommerce.API.Infraestructure
{
    public static class DependencyInjection
    {
        public static void InitializeDependencies(WebApplicationBuilder builder)
        {
            // Business
            builder.Services.AddScoped<IAdministratorBusiness, AdministratorBusiness>();
            builder.Services.AddScoped<IAnnouncementBusiness, AnnouncementBusiness>();
            builder.Services.AddScoped<IApiAccessBusiness, ApiAccessBusiness>();
            builder.Services.AddScoped<IAuthenticationBusiness, AuthenticationBusiness>();
            builder.Services.AddScoped<IEstablishmentBusiness, EstablishmentBusiness>();
            builder.Services.AddScoped<IFavoriteBusiness, FavoriteBusiness>();
            builder.Services.AddScoped<IFooterItemBusiness, FooterItemBusiness>();
            builder.Services.AddScoped<IProductBusiness, ProductBusiness>();
            builder.Services.AddScoped<IReportBusiness, ReportBusiness>();
            builder.Services.AddScoped<IResumeBusiness, ResumeBusiness>();
            builder.Services.AddScoped<INewOrderBusiness, NewOrderBusiness>();
            builder.Services.AddScoped<IOrderBusiness, OrderBusiness>();
            builder.Services.AddScoped<IUserBusiness, UserBusiness>();

            // Repository
            builder.Services.AddScoped<IAdministratorRepository, AdministratorRepository>();
            builder.Services.AddScoped<IAnnouncementRepository, AnnouncementRepository>();
            builder.Services.AddScoped<IApiAccessRepository, ApiAccessRepository>();
            builder.Services.AddScoped<IEstablishmentRepository, EstablishmentRepository>();
            builder.Services.AddScoped<IFavoriteRepository, FavoriteRepository>();
            builder.Services.AddScoped<IFooterItemRepository, FooterItemRepository>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IReportRepository, ReportRepository>();
            builder.Services.AddScoped<INewOrderRepository, NewOrderRepository>();
            //builder.Services.AddScoped<IResumeRepository, ResumeRepository>();
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();

            // Services
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<IS3FileHandler, S3FileHandler>();
            builder.Services.AddScoped<IAddressService, AddressService>();
        }
    }
}
