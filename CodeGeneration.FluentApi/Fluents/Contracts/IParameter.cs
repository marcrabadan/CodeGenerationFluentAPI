using System;
using CodeGeneration.FluentApi.Enums;

namespace CodeGeneration.FluentApi.Fluents.Contracts
{
    public interface IParameter
    {
        IParameter Param(string name, Type type, ParameterDirection direction);
        IParameter Param(string name, string typeName, ParameterDirection direction);
    }
}
