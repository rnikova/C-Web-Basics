using System;
using System.Linq;
using SIS.WebServer;
using SIS.HTTP.Enums;
using System.Reflection;
using SIS.HTTP.Requests;
using SIS.HTTP.Responses;
using SIS.WebServer.Routing;
using SIS.MvcFramework.Logging;
using SIS.MvcFramework.Sessions;
using System.Collections.Generic;
using SIS.MvcFramework.Attributes.Http;
using SIS.MvcFramework.Attributes.Action;
using SIS.MvcFramework.Attributes.Security;
using SIS.MvcFramework.DependencyContainer;

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
                ISet<string> httpDataValue = TryGetHttpParameter(request, parameter.Name);

                if (parameter.ParameterType.GetInterfaces().Any(x =>
                        x.IsGenericType
                        && x.GetGenericTypeDefinition() == typeof(IEnumerable<>)))
                {
                    var collection = httpDataValue.Select(x => Convert.ChangeType(x, parameter.ParameterType.GenericTypeArguments.First()));
                    parameterValues.Add(collection);
                    continue;
                }

                try
                {
                    var httpStringValue = httpDataValue.FirstOrDefault();
                    var parameterValue = Convert.ChangeType(httpStringValue, parameter.ParameterType);
                    parameterValues.Add(parameterValue);
                }
                catch 
                {
                    var paramaterValue = Activator.CreateInstance(parameter.ParameterType);
                    var properties = parameter.ParameterType.GetProperties();

                    foreach (var property in properties)
                    {
                        ISet<string> propertyHttpDataValue = TryGetHttpParameter(request, property.Name);

                        if (property.PropertyType
                            .GetInterfaces()
                            .Any(x => x.IsGenericType 
                                && x.GetGenericTypeDefinition() == typeof(IEnumerable<>) 
                                && property.PropertyType != typeof(string)))
                        {
                            var propertyValue = (IList<string>)Activator.CreateInstance(property.PropertyType);

                            foreach (var parameterElement in propertyHttpDataValue)
                            {
                                propertyValue.Add(parameterElement);
                            }

                            property.SetMethod.Invoke(paramaterValue, new object[] { propertyValue });
                        }
                        else
                        {
                            var firstValue = propertyHttpDataValue.First();
                            var propertyValue = Convert.ChangeType(firstValue, property.PropertyType);

                            property.SetMethod.Invoke(paramaterValue, new object[] { propertyValue });
                        }
                    }

                    parameterValues.Add(paramaterValue);
                }

            }

            var responce = action.Invoke(controllerInstance, parameterValues.ToArray()) as IHttpResponse;

            return responce;
        }

        private static ISet<string> TryGetHttpParameter(IHttpRequest request, string parameterName)
        {
            parameterName = parameterName?.ToLower();
            ISet<string> httpDataValue = null;

            if (request.QueryData.Any(x => x.Key.ToLower() == parameterName))
            {
                httpDataValue = request.QueryData.FirstOrDefault(x => x.Key.ToLower() == parameterName).Value;
            }
            else if (request.FormData.Any(x => x.Key.ToLower() == parameterName))
            {
                httpDataValue = request.FormData.FirstOrDefault(x => x.Key.ToLower() == parameterName).Value;
            }

            return httpDataValue;
        }
    }
}
