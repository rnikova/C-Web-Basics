using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SIS.MvcFramework.DependencyContainer
{
    public class ServiceProvider : IServiceProvider
    {
        private readonly IDictionary<Type, Type> dependdencyContainer = new ConcurrentDictionary<Type, Type>();

        public void Add<TSource, TDestination>()
            where TDestination : TSource
        {
            this.dependdencyContainer[typeof(TSource)] = typeof(TDestination);
        }

        public object CreateInstance(Type type)
        {
            if (dependdencyContainer.ContainsKey(type))
            {
                type = dependdencyContainer[type];
            }

            var constructor = type
                .GetConstructors(BindingFlags.Public | BindingFlags.Instance)
                .OrderBy(x => x.GetParameters().Count())
                .FirstOrDefault();

            if (constructor == null)
            {
                return null;
            }

            var parameters = constructor.GetParameters();
            var parameterInstances = new List<object>();

            foreach (var parameter in parameters)
            {
                var parameterInstance = CreateInstance(parameter.ParameterType);
                parameterInstances.Add(parameterInstance);
            }

            var obj = constructor.Invoke(parameterInstances.ToArray());

            return obj;
        }
    }
}
