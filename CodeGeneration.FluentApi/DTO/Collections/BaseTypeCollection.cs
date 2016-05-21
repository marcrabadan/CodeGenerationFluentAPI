using System.CodeDom;
using System.Collections.ObjectModel;
using System.Linq;
using CodeGeneration.FluentApi.DTO.Members;
using CodeGeneration.FluentApi.Extensions;

namespace CodeGeneration.FluentApi.DTO.Collections
{
    public class BaseTypeCollection : Collection<BaseType>
    {
        public CodeTypeReferenceCollection ToCollection()
        {
            CodeTypeReferenceCollection collection = new CodeTypeReferenceCollection();
            if (this.Any())
            {
                collection.AddRange(this.Select(c =>
                {
                    if (c.Type != null)
                        return new CodeTypeReference(c.Type.ToCSharpStringFormat());
                    else
                        return new CodeTypeReference(c.TypeName);
                }).ToArray());
            }
            return collection;
        }

    }
}
