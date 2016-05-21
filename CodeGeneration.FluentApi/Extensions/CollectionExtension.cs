using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeGeneration.FluentApi.Extensions
{
    public static class CollectionExtension
    {
        public static IEnumerable<TOutPut> Distinct<TOutPut>(this IEnumerable<TOutPut> list, Func<TOutPut, object> propertySelector)
        {
            return list.GroupBy(propertySelector).Select(x => x.First());
        }
    }
}
