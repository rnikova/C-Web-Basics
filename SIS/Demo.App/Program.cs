using SIS.WebServer;
using SIS.HTTP.Enums;
using Demo.App.Controllers;
using SIS.WebServer.Routing;
using SIS.WebServer.Routing.Contracts;

namespace Demo.App
{
    class Program
    {
        static void Main(string[] args)
        {
            IServerRoutingTable serverRoutingTable = new ServerRoutingTable();

            serverRoutingTable.Add(HttpRequestMethod.Get, "/", httpRequest 
                => new HomeController().Home(httpRequest));
            
            serverRoutingTable.Add(HttpRequestMethod.Get, "/login", httpRequest 
                => new HomeController().Login(httpRequest));

            Server server = new Server(8000, serverRoutingTable);
            server.Run();
        }
    }
}
