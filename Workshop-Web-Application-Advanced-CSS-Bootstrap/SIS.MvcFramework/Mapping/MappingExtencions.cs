using System.Collections.Generic;
using System.Linq;

namespace SIS.MvcFramework.Mapping
{
    public static class MappingExtencions
    {
        public static IEnumerable<TDestination> To<TDestination>(this IEnumerable<object> collection)
        {
            return collection.Select(elem => ModelMapper.ProjectTo<TDestination>(elem)).ToList();
        }
    }
}
