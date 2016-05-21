using System;
using System.CodeDom;
using CodeGeneration.FluentApi.DTO.Members;
using CodeGeneration.FluentApi.Enums;

namespace CodeGeneration.FluentApi.Fluents.Contracts
{
    public interface IConstructor
    {
        IConstructor AddAttribute(MemberAttributes attributes);

        IConstructor AddCustomAttribute(string name);
        IConstructor AddCustomAttribute(string name, IAttributeParameter[] parameters);
        
        IConstructor AddParameter(string name, Type type, ParameterDirection direction);
        IConstructor AddParameter(string name, string typeName, ParameterDirection direction);

        IConstructor AddStatement(string scriptText);
        IConstructor AddStatement(Statement statement);
        IConstructor AddStatement(CodeStatement statement);
    }
}
