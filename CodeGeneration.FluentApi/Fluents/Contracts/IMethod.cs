using System;
using System.CodeDom;
using System.Collections.Generic;
using CodeGeneration.FluentApi.DTO.Members;
using CodeGeneration.FluentApi.Enums;

namespace CodeGeneration.FluentApi.Fluents.Contracts
{
    public interface IMethod
    {
        IMethod MethodStatement(MemberAttributes modifiers, string name);
        
        IMethod Void();
        IMethod WithType(Type returnType);
        IMethod WithType(string returnTypeName);

        IMethod AddCustomAttribute(string name, IAttributeParameter[] parameters);

        IMethod AddParameter(string name, Type type, ParameterDirection direction);
        IMethod AddParameter(string name, string typeName, ParameterDirection direction);

        IMethod AddStatement(string scriptText);
        IMethod AddStatement(Statement statement);
        IMethod AddStatement(CodeStatement statement);
        IMethod AddStatements(IEnumerable<CodeStatement> statements);
    }
}
