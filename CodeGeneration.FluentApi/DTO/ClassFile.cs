using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.CSharp;
using CodeGeneration.FluentApi.Enums;
using CodeGeneration.FluentApi.DTO.Collections;
using CodeGeneration.FluentApi.DTO.Members;
using CodeGeneration.FluentApi.Fluents.Contracts;
using CodeGeneration.FluentApi.Fluents.Members;
using CodeGeneration.FluentApi.Snippets;

namespace CodeGeneration.FluentApi.DTO
{
    public class ClassFile : IClassFile
    {
        #region .: Fields :.


        #endregion

        #region .: Constructors :.

        public ClassFile()
        {
            BaseTypes = new BaseTypeCollection();
            Imports = new ImportCollection();
            Constructors = new ConstructorCollection();
            CustomAttributes = new CustomAttributeCollection();
            Fields = new FieldCollection();
            Properties = new PropertyCollection();
            Methods = new MethodCollection();

            CodeFileType = CodeFileType.Class;
        }

        #endregion
        
        #region .: Public Properties :.

        public string ClassName { get; set; }
        public string Namespace { get; set; }
        public BaseTypeCollection BaseTypes { get; set; }
        public ImportCollection Imports { get; set; }
        public CodeFileType CodeFileType { get; set; }
        public ConstructorCollection Constructors { get; set; }
        public TypeAttributes Attributes { get; set; }
        public CustomAttributeCollection CustomAttributes { get; set; }
        public FieldCollection Fields { get; set; }
        public PropertyCollection Properties { get; set; }
        public MethodCollection Methods { get; set; }

        #endregion 

        #region .: Public Methods :.

        public IClassFile InNamespace(string nameSpace)
        {
            Namespace = nameSpace;
            return this;
        }

        public IClassFile AddImport(Type type)
        {
            Imports.Add((Import)MemberFluentFactory.InitImport().ImportStatement(type));
            return this;
        }

        public IClassFile AddImport(string typeName)
        {
            Imports.Add((Import)MemberFluentFactory.InitImport().ImportStatement(typeName));
            return this;
        }

        public IClassFile AddCustomAttribute(string name)
        {
            CustomAttributes.Add((CustomAttribute)MemberFluentFactory.InitCustomAttribute().Name(name));
            return this;
        }

        public IClassFile AddCustomAttribute(string name, IAttributeParameter[] parameters)
        {
            var customAttribute = MemberFluentFactory.InitCustomAttribute().Name(name);
            parameters.ToList().ForEach(c => customAttribute.AddAttribute(c));
            CustomAttributes.Add((CustomAttribute)customAttribute);
            return this;
        }

        public IClassFile AddProperty(IProperty property)
        {
            Properties.Add((Property)property);
            return this;
        }

        public IClassFile AddProperty(MemberAttributes modifiers, string propertyName, Type propertyType)
        {
            var property = MemberFluentFactory
                .InitProperty()
                .Name(propertyName)
                .Type(propertyType)
                .Attributes(modifiers);
            Properties.Add((Property)property);
            return this;
        }

        public IClassFile AddMethod(IMethod method)
        {
            Methods.Add((Method)method);
            return this;
        }

        public IClassFile AddMethod(MemberAttributes modifiers, string name, Type type)
        {
            var method = MemberFluentFactory.InitMethod().MethodStatement(modifiers, name).WithType(type);
            method = method.AddStatement(Statements.ThrowExceptionStatement(Type.GetType("System.NotImplementedException")));
            Methods.Add((Method)method);
            return this;
        }

        public IClassFile AddMethod(MemberAttributes modifiers, string name, string typeName)
        {
            var method = MemberFluentFactory.InitMethod().MethodStatement(modifiers, name).WithType(typeName);
            method = method.AddStatement(Statements.ThrowExceptionStatement(Type.GetType("System.NotImplementedException")));
            Methods.Add((Method)method);
            return this;
        }

        public IClassFile AddMethod(MemberAttributes modifiers, string name, Type type, IParameter[] parameters)
        {
            var method = MemberFluentFactory.InitMethod().MethodStatement(modifiers, name).WithType(type);
            method = method.AddStatement(Statements.ThrowExceptionStatement(Type.GetType("System.NotImplementedException")));
            parameters.OfType<Parameter>().ToList().ForEach(c =>
            {
                if (c.ParameterType == null)
                    method.AddParameter(c.ParameterName, c.ParameterTypeName, c.ParameterDirection);
                else
                    method.AddParameter(c.ParameterName, c.ParameterType, c.ParameterDirection);
            });
            Methods.Add((Method)method);
            return this;
        }

        public IClassFile AddMethod(MemberAttributes modifiers, string name, string typeName, IParameter[] parameters)
        {
            var method = MemberFluentFactory.InitMethod().MethodStatement(modifiers, name).WithType(typeName);
            method = method.AddStatement(Statements.ThrowExceptionStatement(Type.GetType("System.NotImplementedException")));
            parameters.OfType<Parameter>().ToList().ForEach(c =>
            {
                if (c.ParameterType == null)
                    method.AddParameter(c.ParameterName, c.ParameterTypeName, c.ParameterDirection);
                else
                    method.AddParameter(c.ParameterName, c.ParameterType, c.ParameterDirection);
            });
            Methods.Add((Method)method);
            return this;
        }

        public IClassFile AddVoidMethod(MemberAttributes modifiers, string name, IParameter[] parameters)
        {
            var method = MemberFluentFactory.InitMethod().MethodStatement(modifiers, name).Void();
            method = method.AddStatement(Statements.ThrowExceptionStatement(Type.GetType("System.NotImplementedException")));
            parameters.OfType<Parameter>().ToList().ForEach(c =>
            {
                if (c.ParameterType == null)
                    method.AddParameter(c.ParameterName, c.ParameterTypeName, c.ParameterDirection);
                else
                    method.AddParameter(c.ParameterName, c.ParameterType, c.ParameterDirection);
            });
            Methods.Add((Method)method);
            return this;
        }

        public IClassFile AddBaseType(Type type)
        {
            BaseTypes.Add((BaseType)MemberFluentFactory.InitBaseType().BaseTypeStatement(type));
            return this;
        }

        public IClassFile AddBaseType(string typeName)
        {
            BaseTypes.Add((BaseType)MemberFluentFactory.InitBaseType().BaseTypeStatement(typeName));
            return this;
        }

        public IClassFile AddAttribute(TypeAttributes attributes)
        {
            Attributes = attributes;
            return this;
        }

        public IClassFile AddConstructor(IConstructor contructor)
        {
            Constructors.Add((Constructor)contructor);
            return this;
        }

        public IClassFile AddField(string name, Type type)
        {
            Fields.Add((Field)MemberFluentFactory.InitField().FieldStatement(name,type));
            return this;
        }

        public IClassFile AddField(IField field)
        {
            Fields.Add((Field)field);
            return this;
        }

        public IClassFile AddField(string name, string typeName)
        {
            Fields.Add((Field)MemberFluentFactory.InitField().FieldStatement(name, typeName));
            return this;
        }

        public IClassFile Name(string name)
        {
            ClassName = name;
            return this;
        }

        public void SaveTo(string path)
        {
            CodeUnit codeUnit = new CodeUnit(new CSharpCodeProvider())
            {
                ClassFile = this,
                SaveAs = path
            };
            codeUnit.SaveCSharpFile();
        }

        public ICompiler ToCompile()
        {
            return (ICompiler)new Compiler(this);
        }

        #endregion
    }
}
