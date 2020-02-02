using IRunes.Data;
using SIS.MvcFramework;
using SIS.WebServer.Routing;
using SIS.MvcFramework.DependencyContainer;
using IRunes.Services;

namespace IRunes.App
{
    public class StartUp : IMvcApplication
    {
        public void Configure(IServerRoutingTable serverRoutingTable)
        {
            using (var context = new RunesDbContext())
            {
                context.Database.EnsureCreated();
            }

           // serverRoutingTable.Add(HttpRequestMethod.Get, "/Info/About", request => new InfoController().About(request));   
        }

        public void ConfigureServices(IServiceProvider serviceProvider)
        {
            serviceProvider.Add<IAlbumService, AlbumService>();
            serviceProvider.Add<ITrackService, TrackService>();
            serviceProvider.Add<IUserService, UserService>();
        }
    }
}
