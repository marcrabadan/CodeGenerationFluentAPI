using System;

namespace CodeGeneration.FluentApi.Fluents.Contracts
{
    public interface IImport
    {
        IImport ImportStatement(Type type);
        IImport ImportStatement(string typeName);
    }
}
