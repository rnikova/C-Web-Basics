using System;
using System.Linq;
using SIS.WebServer;
using SIS.HTTP.Enums;
using System.Reflection;
using SIS.HTTP.Responses;
using SIS.WebServer.Routing;
using SIS.MvcFramework.Sessions;
using SIS.MvcFramework.Attributes.Http;
using SIS.MvcFramework.Attributes.Action;
using SIS.MvcFramework.Attributes.Security;
using SIS.MvcFramework.DependencyContainer;
using SIS.MvcFramework.Logging;
using SIS.HTTP.Requests;
using System.Collections.Generic;

namespace SIS.MvcFramework
{
    public static class WebHost
    {
        public static void Start(IMvcApplication application)
        {
            IServerRoutingTable serverRoutingTable = new ServerRoutingTable();
            IHttpSessionStorage httpSessionStorage = new HttpSessionStorage();
            DependencyContainer.IServiceProvider serviceProvider = new ServiceProvider();
            serviceProvider.Add<ILogger, ConsoleLogger>();

            AutoRegisterRoutes(application, serverRoutingTable, serviceProvider);

            application.ConfigureServices(serviceProvider);
            application.Configure(serverRoutingTable);

            Server server = new Server(8000, serverRoutingTable, httpSessionStorage);
            server.Run();
        }

        private static void AutoRegisterRoutes(IMvcApplication application,
            IServerRoutingTable serverRoutingTable,
            DependencyContainer.IServiceProvider serviceProvider)
        {
            var controllers = application
                .GetType()
                .Assembly
                .GetTypes()
                .Where(type => type.IsClass && !type.IsAbstract && typeof(Controller).IsAssignableFrom(type));

            foreach (var controllerType in controllers)
            {
                var actions = controllerType
                    .GetMethods(BindingFlags.DeclaredOnly
                        | BindingFlags.Public
                        | BindingFlags.Instance)
                    .Where(x => !x.IsSpecialName && x.DeclaringType == controllerType)
                    .Where(x => x.GetCustomAttributes().All(a => a.GetType() != typeof(NonActionAttribute)));

                foreach (var action in actions)
                {
                    var path = $"/{controllerType.Name.Replace("Controller", string.Empty)}/{action.Name}";
                    var attribute = action
                        .GetCustomAttributes()
                        .Where(x => x.GetType().IsSubclassOf(typeof(BaseHttpAttribute)))
                        .LastOrDefault()
                        as BaseHttpAttribute;

                    var httpMethod = HttpRequestMethod.Get;

                    if (attribute != null)
                    {
                        httpMethod = attribute.Method;
                    }

                    if (attribute?.Url != null)
                    {
                        path = attribute.Url;
                    }

                    if (attribute?.ActionName != null)
                    {
                        path = $"/{controllerType.Name.Replace("Controller", string.Empty)}/{attribute.ActionName}";
                    }

                    serverRoutingTable.Add(httpMethod, path, (request) => ProccessRequest(serviceProvider, controllerType, action, request));

                    Console.WriteLine(httpMethod + " " + path);
                }
            }
        }

        private static IHttpResponse ProccessRequest(
            DependencyContainer.IServiceProvider serviceProvider,
            Type controllerType,
            MethodInfo action,
            IHttpRequest request)
        {
            var controllerInstance = serviceProvider.CreateInstance(controllerType);
            ((Controller)controllerInstance).Request = request;
            var controllerPrincipal = ((Controller)controllerInstance).User;
            var authorizeAttribute = action.GetCustomAttributes()
            .LastOrDefault(a => a.GetType() == typeof(AuthorizeAttribute)) as AuthorizeAttribute;

            if (authorizeAttribute != null && !authorizeAttribute.IsInAuthority(controllerPrincipal))
            {
                return new HttpResponse(HttpResponseStatusCode.Forbidden);
            }

            var parameters = action.GetParameters();
            var parameterValues = new List<object>();

            foreach (var parameter in parameters)
            {
                var parameterName = parameter.Name.ToLower();
                object parameterValue = null;

                if (request.QueryData.Any(x => x.Key.ToLower() == parameterName))
                {
                    parameterValue = request.QueryData.FirstOrDefault(x => x.Key.ToLower() == parameterName);
                }
                
                if (request.FormData.Any(x => x.Key.ToLower() == parameterName))
                {
                    parameterValue = request.FormData.FirstOrDefault(x => x.Key.ToLower() == parameterName);
                }

                //Convert.ChangeType
            }

            var responce = action.Invoke(controllerInstance, parameterValues.ToArray()) as IHttpResponse;

            return responce;
        }
    }
}
