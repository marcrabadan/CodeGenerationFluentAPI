using System.CodeDom;
using System.Linq;

namespace CodeGeneration.FluentApi.DTO.Collections
{
    public class StatementCollection : CodeStatementCollection
    {
        public CodeStatement[] ToArray()
        {
            return this.OfType<CodeStatement>().ToArray();
        }

        public StatementCollection InsertRange(int index, CodeStatement[] codeStatements)
        {
            foreach (var statement in codeStatements)
            {
                index++;
                Insert(index, statement);
            }
            return this;
        }
    }
}
