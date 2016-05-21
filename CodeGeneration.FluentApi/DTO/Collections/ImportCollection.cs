using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CodeGeneration.FluentApi.DTO.Members;

namespace CodeGeneration.FluentApi.DTO.Collections
{
    public class ImportCollection : Collection<Import>
    {
        public CodeNamespaceImport[] ToCollection()
        {
            List<CodeNamespaceImport> collection = new List<CodeNamespaceImport>();
            List<string> codeNamespaceImports = new List<string>();
            if (this.Any())
            {
                codeNamespaceImports.AddRange(this.Select(c =>
                {
                    if (c.Type != null)
                        return c.Type.Namespace;
                    else
                        return c.TypeName;
                }));
            }
            collection.AddRange(codeNamespaceImports.Where(c => !string.IsNullOrEmpty(c)).Distinct().OrderBy(c => c).ToList().Select(c => new CodeNamespaceImport(c)).ToArray());
            return collection.ToArray();
        }
    }
}
