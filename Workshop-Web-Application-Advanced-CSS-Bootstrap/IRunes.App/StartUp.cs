using IRunes.App.Controllers;
using IRunes.Data;
using SIS.HTTP.Enums;
using SIS.MvcFramework;
using SIS.MvcFramework;
using SIS.MvcFramework.Result;
using SIS.MvcFramework.Routing;

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

        public void ConfigureServices()
        {
            
        }
    }
}
