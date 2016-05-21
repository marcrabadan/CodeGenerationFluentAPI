using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using CodeGeneration.FluentApi.DTO.Collections;
using CodeGeneration.FluentApi.Enums;
using CodeGeneration.FluentApi.Fluents.Contracts;
using CodeGeneration.FluentApi.Fluents.Members;

namespace CodeGeneration.FluentApi.DTO.Members
{
    public class Method : IMethod
    {
        #region .: Variables :. 



        #endregion

        #region .: Constructor :.

        public Method()
        {
            CustomAttributes = new CustomAttributeCollection();
            Parameters = new ParameterCollection();
            Statements = new StatementCollection();
        }

        #endregion

        #region .: Public Properties :.

        public string Name { get; set; }
        public Type ReturnType { get; set; }
        public string ReturnTypeName { get; set; }
        public bool IsVoid { get; set; }
        public MemberAttributes Attributes { get; set; }
        public CustomAttributeCollection CustomAttributes { get; set; }
        public ParameterCollection Parameters { get; set; }
        public StatementCollection Statements { get; set; }

        #endregion

        #region .: Public Methods :.



        #endregion

        public IMethod MethodStatement(MemberAttributes modifiers, string name)
        {
            Attributes = modifiers;
            Name = name;
            return this;
        }

        public IMethod Void()
        {
            IsVoid = true;
            return this;
        }

        public IMethod NotVoid()
        {
            IsVoid = false;
            return this;
        }

        public IMethod WithType(Type returnType)
        {
            ReturnType = returnType;
            return this;
        }

        public IMethod WithType(string returnTypeName)
        {
            ReturnTypeName = returnTypeName;
            return this;
        }

        public IMethod AddCustomAttribute(string name, IAttributeParameter[] parameters)
        {
            var customAttribute = MemberFluentFactory.InitCustomAttribute().Name(name);
            parameters.ToList().ForEach(c => customAttribute.AddAttribute(c));
            CustomAttributes.Add((CustomAttribute)customAttribute);
            return this;
        }

        public IMethod AddParameter(string name, Type type, ParameterDirection direction)
        {
            Parameters.Add((Parameter)MemberFluentFactory.InitParameter().Param(name, type, direction));
            return this;
        }

        public IMethod AddParameter(string name, string typeName, ParameterDirection direction)
        {
            Parameters.Add((Parameter)MemberFluentFactory.InitParameter().Param(name, typeName, direction));
            return this;
        }

        public IMethod AddStatement(string scriptText)
        {
            Statements.Add(new CodeSnippetStatement(scriptText));
            return this;
        }

        public IMethod AddStatement(Statement statement)
        {
            if (statement != null) Statements.Add(statement);
            return this;
        }

        public IMethod AddStatement(CodeStatement statement)
        {
            if (statement != null) Statements.Add(statement);
            return this;
        }

        public IMethod AddStatements(IEnumerable<CodeStatement> statements)
        {
            if (statements != null && statements.Any()) Statements.AddRange(statements.ToArray());
            return this;
        }
    }
}
