using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CodeGeneration.FluentApi.DTO.Members;

namespace CodeGeneration.FluentApi.DTO.Collections
{
    public class AttributeArgumentCollection : Collection<AttributeParameter>
    {
        public CodeAttributeArgument[] ToCollection()
        {
            List<CodeAttributeArgument> collection = new List<CodeAttributeArgument>();
            if (this.Any())
            {
                collection.AddRange(this.Select(c => new CodeAttributeArgument(c.ArgumentName, new CodePrimitiveExpression(c.ArgumentValue))));
            }
            return collection.ToArray();
        }
    }
}
