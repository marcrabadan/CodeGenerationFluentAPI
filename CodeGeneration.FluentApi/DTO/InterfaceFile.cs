using System;
using System.CodeDom;
using System.Linq;
using System.Reflection;
using Microsoft.CSharp;
using CodeGeneration.FluentApi.DTO.Collections;
using CodeGeneration.FluentApi.DTO.Members;
using CodeGeneration.FluentApi.Enums;
using CodeGeneration.FluentApi.Fluents.Contracts;
using CodeGeneration.FluentApi.Fluents.Members;

namespace CodeGeneration.FluentApi.DTO
{
    public class InterfaceFile : IInterfaceFile
    {
        #region .: Variables :. 



        #endregion

        #region .: Constructor :.

        public InterfaceFile()
        {
            Imports = new ImportCollection();
            Methods = new MethodCollection();

            CodeFileType = CodeFileType.Interface;
        }

        #endregion

        #region .: Public Properties :.

        public string InterfaceName { get; set; }
        public string Namespace { get; set; }
        public TypeAttributes Attributes { get; set; }
        public ImportCollection Imports { get; set; }
        public CodeFileType CodeFileType { get; set; }
        public MethodCollection Methods { get; set; }

        #endregion
        
        #region .: Public Methods :.

        public IInterfaceFile Name(string name)
        {
            InterfaceName = name;
            return this;
        }

        public IInterfaceFile AddAttribute(TypeAttributes attributes)
        {
            Attributes = attributes;
            return this;
        }

        public IInterfaceFile InNamespace(string nameSpace)
        {
            Namespace = nameSpace;
            return this;
        }

        public IInterfaceFile AddImport(Type type)
        {
            Imports.Add((Import)MemberFluentFactory.InitImport().ImportStatement(type));
            return this;
        }

        public IInterfaceFile AddImport(string typeName)
        {
            Imports.Add((Import)MemberFluentFactory.InitImport().ImportStatement(typeName));
            return this;
        }

        public IInterfaceFile AddMethod(IMethod method)
        {
            Methods.Add((Method)method);
            return this;
        }

        public IInterfaceFile AddMethod(MemberAttributes modifiers, string name, Type type)
        {
            Methods.Add((Method)MemberFluentFactory.InitMethod().MethodStatement(modifiers, name).WithType(type));
            return this;
        }

        public IInterfaceFile AddMethod(MemberAttributes modifiers, string name, string typeName)
        {
            Methods.Add((Method)MemberFluentFactory.InitMethod().MethodStatement(modifiers, name).WithType(typeName));
            return this;
        }

        public IInterfaceFile AddMethod(MemberAttributes modifiers, string name, Type type, IParameter[] parameters)
        {
            var method = MemberFluentFactory.InitMethod().MethodStatement(modifiers, name).WithType(type);
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

        public IInterfaceFile AddMethod(MemberAttributes modifiers, string name, string typeName, IParameter[] parameters)
        {
            var method = MemberFluentFactory.InitMethod().MethodStatement(modifiers, name).WithType(typeName);
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

        public IInterfaceFile AddVoidMethod(MemberAttributes modifiers, string name, IParameter[] parameters)
        {
            var method = MemberFluentFactory.InitMethod().MethodStatement(modifiers, name).Void();
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

        public void SaveTo(string path)
        {
            CodeUnit codeUnit = new CodeUnit(new CSharpCodeProvider())
            {
                ClassFile = new ClassFile
                {
                    ClassName = InterfaceName,
                    Imports = Imports,
                    Attributes = Attributes,
                    CodeFileType = CodeFileType,
                    Namespace = Namespace,
                    Methods = Methods
                },
                SaveAs = path
            };
            codeUnit.SaveCSharpFile();
        }

        #endregion


    }
}
