using System.CodeDom;
using System.Collections.ObjectModel;
using System.Linq;
using CodeGeneration.FluentApi.DTO.Members;
using CodeGeneration.FluentApi.Extensions;

namespace CodeGeneration.FluentApi.DTO.Collections
{
    public class ParameterCollection : Collection<Parameter>
    {
        public CodeParameterDeclarationExpressionCollection ToCollection()
        {
            CodeParameterDeclarationExpressionCollection collection = new CodeParameterDeclarationExpressionCollection();
            if (this.Any())
            {
                collection.AddRange(this.Select(c =>
                {
                    if (c.ParameterType != null)
                        return new CodeParameterDeclarationExpression(c.ParameterType.ToCSharpStringFormat(), c.ParameterName);
                    else
                       return new CodeParameterDeclarationExpression(c.ParameterTypeName, c.ParameterName);
                }).ToArray());
            }
            return collection;
        }
    }
}
