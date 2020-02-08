using SIS.MvcFramework;
using SIS.MvcFramework.Routing;
using SIS.MvcFramework.DependencyContainer;
using Panda.Data;
using Panda.Services;

namespace Panda.Web
{
    public class Startup : IMvcApplication
    {
        public void Configure(IServerRoutingTable serverRoutingTable)
        {
            using (var db = new PandaDbContext())
            {
                db.Database.EnsureCreated();
            }
        }

        public void ConfigureServices(IServiceProvider serviceProvider)
        {
            serviceProvider.Add<IUsersService, UsersService>();
        }
    }
}
