using System.CodeDom;
using System.Collections.ObjectModel;
using System.Linq;
using CodeGeneration.FluentApi.DTO.Members;

namespace CodeGeneration.FluentApi.DTO.Collections
{
    public class CustomAttributeCollection : Collection<CustomAttribute>
    {
        public CodeAttributeDeclarationCollection ToCollection()
        {
            CodeAttributeDeclarationCollection collection = new CodeAttributeDeclarationCollection();
            if (this.Any())
            {
                collection.AddRange(this.Select(c =>
                {
                    if (c.CustomAttributeArguments != null && c.CustomAttributeArguments.Any())
                        return new CodeAttributeDeclaration(c.CustomAttributeName, c.CustomAttributeArguments.ToCollection());
                    else
                        return new CodeAttributeDeclaration(c.CustomAttributeName);
                }).ToArray());
            }
            return collection;
        }
    }
}
