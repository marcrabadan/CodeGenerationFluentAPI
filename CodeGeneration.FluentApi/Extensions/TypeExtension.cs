using System;
using System.Linq;
using System.Text;
namespace CodeGeneration.FluentApi.Extensions
{
    public static class TypeExtension
    {
        public static string ToCSharpStringFormat(this Type t)
        {
            if (!t.IsGenericType)
                return t.Name;
            StringBuilder sb = new StringBuilder();

            sb.Append(t.Name.Substring(0, t.Name.LastIndexOf("`", StringComparison.InvariantCulture)));
            
            sb.Append(
                t
                .GetGenericArguments()
                .Aggregate("<",
                    (aggregate, type) => aggregate + (aggregate == "<" ? "" : ",") + type.ToCSharpStringFormat()
            ));

            sb.Append(">");

            return sb.ToString();
        }
    }
}
