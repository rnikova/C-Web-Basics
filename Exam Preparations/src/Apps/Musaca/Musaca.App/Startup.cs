using SIS.MvcFramework;
using SIS.MvcFramework.Routing;
using SIS.MvcFramework.DependencyContainer;
using Musaca.Data;
using Musaca.Services;

namespace Musaca.App
{
    public class Startup : IMvcApplication
    {
        public void Configure(IServerRoutingTable serverRoutingTable)
        {
            using (var db = new MusacaDbContext())
            {
                db.Database.EnsureCreated();
            }
        }

        public void ConfigureServices(IServiceProvider serviceProvider)
        {
            serviceProvider.Add<IUsersService, UsersService>();
            serviceProvider.Add<IProductsService, ProductsService>();
            serviceProvider.Add<IOrdersService, OrdersService>();
        }
    }
}
