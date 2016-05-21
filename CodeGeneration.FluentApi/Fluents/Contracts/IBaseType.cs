using System;

namespace CodeGeneration.FluentApi.Fluents.Contracts
{
    public interface IBaseType
    {
        IBaseType BaseTypeStatement(Type type);
        IBaseType BaseTypeStatement(string typeName);
    }
}
