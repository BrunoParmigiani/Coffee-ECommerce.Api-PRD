using Coffee_Ecommerce.API.Infraestructure;
using Microsoft.EntityFrameworkCore;

namespace Coffee_Ecommerce.API.Services
{
    public static class DbHelper
    {
        private static PostgreContext _context;

        public static void Configure(PostgreContext context)
        {
            _context = context;
        }

        public static void ExecuteSql(FormattableString query)
        {
            _context.Database.ExecuteSql(query);
        }
    }
}
