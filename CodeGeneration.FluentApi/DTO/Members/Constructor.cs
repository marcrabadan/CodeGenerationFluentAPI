using System;
using System.CodeDom;
using System.Linq;
using CodeGeneration.FluentApi.DTO.Collections;
using CodeGeneration.FluentApi.Enums;
using CodeGeneration.FluentApi.Fluents.Contracts;
using CodeGeneration.FluentApi.Fluents.Members;
using CodeGeneration.FluentApi.Snippets;

namespace CodeGeneration.FluentApi.DTO.Members
{
    public class Constructor : IConstructor
    {
        #region .: Variables :. 



        #endregion

        #region .: Constructor :.

        public Constructor()
        {
            ConstructorCustomAttributes = new CustomAttributeCollection();
            ConstructorParameters = new ParameterCollection();
            ConstructorStatements = new StatementCollection();
        }

        #endregion

        #region .: Public Property :.

        public MemberAttributes ConstructorAttributes { get; set; }
        public CustomAttributeCollection ConstructorCustomAttributes { get; set; }
        public ParameterCollection ConstructorParameters { get; set; }
        public StatementCollection ConstructorStatements { get; set; }

        #endregion

        #region .: Public Methods :.

        public IConstructor AddAttribute(MemberAttributes attributes)
        {
            ConstructorAttributes = attributes;
            return this;
        }

        public IConstructor AddCustomAttribute(string name)
        {
            ConstructorCustomAttributes.Add((CustomAttribute)MemberFluentFactory.InitCustomAttribute().Name(name));
            return this;
        }

        public IConstructor AddCustomAttribute(string name, IAttributeParameter[] parameters)
        {
            var customAttribute = MemberFluentFactory.InitCustomAttribute().Name(name);
            parameters.ToList().ForEach(c => customAttribute.AddAttribute(c));
            ConstructorCustomAttributes.Add((CustomAttribute)customAttribute);
            return this;
        }

        public IConstructor AddParameter(string name, Type type, ParameterDirection direction)
        {
            ConstructorParameters.Add((Parameter)MemberFluentFactory.InitParameter().Param(name, type, direction));
            return this;
        }

        public IConstructor AddParameter(string name, string typeName, ParameterDirection direction)
        {
            ConstructorParameters.Add((Parameter)MemberFluentFactory.InitParameter().Param(name, typeName, direction));
            return this;
        }

        public IConstructor AddStatement(string scriptText)
        {
            ConstructorStatements.Add(new CodeSnippetStatement(scriptText));
            return this;
        }

        public IConstructor AddStatement(Statement statement)
        {
            if(statement != null) ConstructorStatements.Add(statement);
            return this;
        }

        public IConstructor AddStatement(CodeStatement statement)
        {
            if (statement != null) ConstructorStatements.Add(statement);
            return this;
        }

        #endregion

       
    }
}
