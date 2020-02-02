using SIS.WebServer.Routing;
using SIS.MvcFramework.DependencyContainer;

namespace SIS.MvcFramework
{
    public interface IMvcApplication
    {
        void Configure(IServerRoutingTable serverRoutingTable);

        void ConfigureServices(IServiceProvider serviceProvider);
    }
}
