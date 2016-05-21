using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CodeGeneration.FluentApi.DTO;
using CodeGeneration.FluentApi.Enums;
using CodeGeneration.FluentApi.Fluents;
using CodeGeneration.FluentApi.Fluents.Contracts;
using CodeGeneration.FluentApi.Fluents.Members;
using CodeGeneration.FluentApi.IntTest.MockDTOs;
using CodeGeneration.FluentApi.IntTest.MockEntities;
using CodeGeneration.FluentApi.Snippets;
using CodeGeneration.FluentApi.DTO.Members;
using System.IO;

namespace CodeGeneration.FluentApi.IntTest.GivenCreatingFluent
{
    [TestClass]
    public class FluentApi
    {
        [TestMethod]
        public void MakeClass()
        {
            FluentFactory
                .InitClass()
                .Name("MapperGenerationService")
                .AddImport("System")
                .AddImport(typeof(IEnumerable<ClassFile>))
                .AddImport(typeof(ClassFile))
                .AddImport(typeof(CodeOutputFormat))
                .AddImport(typeof(CodeGenerationSettings))
                .AddImport(typeof(Func<ClassFile, bool>))
                .AddAttribute(TypeAttributes.Public)
                .AddBaseType("IMapperGenerationService")
                .InNamespace("CodeGeneration.Impl.ServiceLibrary.Services")
                .AddConstructor(
                    MemberFluentFactory
                        .InitConstructor()
                        .AddAttribute(MemberAttributes.Public | MemberAttributes.Final)
                        .AddParameter("configuration", "IServiceLibraryConfiguration", ParameterDirection.None)
                        .AddParameter("codeGenerationDomainService", "ICodeGenerationDomainService", ParameterDirection.None)
                        .AddStatement(Statements.AssignStatement("_configuration","configuration"))
                        .AddStatement(Statements.AssignStatement("_codeGenerationDomainService","codeGenerationDomainService")))
                .AddCustomAttribute("RegisterService")
                .AddField("_configuration","IServiceLibraryConfiguration")
                .AddField("_codeGenerationDomainService","ICodeGenerationDomainService")
                .AddMethod(MemberFluentFactory
                    .InitMethod()
                    .MethodStatement(MemberAttributes.Public | MemberAttributes.Final, "GetAll")
                    .AddParameter("filter", typeof(Func<ClassFile, bool>), ParameterDirection.None)
                    .AddStatement(Statements.ThrowExceptionStatement(Type.GetType("System.NotImplementedException"))))
                .AddMethod(MemberFluentFactory
                    .InitMethod()
                    .MethodStatement(MemberAttributes.Public | MemberAttributes.Final, "GenerateCodeFile")
                    .AddParameter("codeFile", typeof(ClassFile), ParameterDirection.None)
                    .AddParameter("outputFormat", typeof(CodeOutputFormat), ParameterDirection.None)
                    .AddParameter("settings", typeof(CodeGenerationSettings), ParameterDirection.None)
                    .AddStatement(Statements.ThrowExceptionStatement(Type.GetType("System.NotImplementedException"))))
                .SaveTo(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
        }

        [TestMethod]
        public void MakeInterface()
        {
            FluentFactory
                .InitInterface()
                .Name("IMapperGenerationService")
                .AddImport("System")
                .AddImport(typeof(ClassFile))
                .AddImport(typeof(CodeOutputFormat))
                .AddImport(typeof(CodeGenerationSettings))
                .AddImport(typeof(Func<ClassFile, bool>))
                .AddAttribute(TypeAttributes.Public | TypeAttributes.Interface)
                .AddMethod(MemberAttributes.Public | MemberAttributes.Final, "GetAll", typeof(IEnumerable<ClassFile>), new []
                {
                    MemberFluentFactory.InitParameter().Param("filter", typeof(Func<ClassFile, bool>), ParameterDirection.None)
                })
                .AddMethod(MemberFluentFactory
                    .InitMethod()
                    .MethodStatement(MemberAttributes.Public | MemberAttributes.Final, "GenerateCodeFile")
                    .WithType(typeof(ClassFile))
                    .AddParameter("codeFile", typeof(ClassFile), ParameterDirection.None)
                    .AddParameter("outputFormat", typeof(CodeOutputFormat), ParameterDirection.None)
                    .AddParameter("settings", typeof(CodeGenerationSettings), ParameterDirection.None))
                .SaveTo(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
        }

        [TestMethod]
        public void MakeMapping()
        {
            FluentFactory
                .InitMapping()
                .Map(typeof(MockDTO), typeof(MockEntity))
                .ResolveMapCollection(new Dictionary<Type, Type>
                {
                    {typeof(CountryDTO),typeof(CountryEntity)}
                }) 
                .Ignore<MockDTO>(c => c.Id)
                .AddProperty<MockEntity, MockDTO>(c => c.AimsName, c => string.Format("{0} ({1})", c.Name, c.NIF.ToString()))
                .AddProperty<MockEntity, MockDTO>(c => c.PhoneNumber, c => string.Format("(+{0}) {1}", c.Country.PrefixNumber, c.Country.Name))
                .SaveTo(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
        }

        [TestMethod]
        public void ConsoleAppCsFile()
        {
            FluentFactory
                .InitClass()
                .InNamespace("DemoApps")
                .Name("ConsoleApp1")
                .AddImport("System")
                .AddImport(typeof(Console).FullName)
                .AddAttribute(TypeAttributes.Public)
                .AddMethod(
                    MemberFluentFactory
                    .InitMethod()
                    .MethodStatement(MemberAttributes.Public | MemberAttributes.Final | MemberAttributes.Static, "Main")
                    .Void()
                    .AddStatement(Statements.MethodInvoke(typeof(Console), "WriteLine", Operators.ObjectValue("Hello World!!!")))
                    .AddStatement(Statements.MethodInvoke(typeof(Console), "ReadLine"))
                ).SaveTo(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
        }

        [TestMethod]
        public void ConsoleAppCompileAndSaveExeFile()
        {
            FluentFactory
                .InitClass()
                .InNamespace("DemoApps")
                .Name("ConsoleApp1")
                .AddImport("System")
                .AddImport(typeof(Console))
                .AddAttribute(TypeAttributes.Public)
                .AddMethod(
                    MemberFluentFactory
                    .InitMethod()
                    .MethodStatement(MemberAttributes.Public | MemberAttributes.Final | MemberAttributes.Static, "Main")
                    .Void()
                    .AddStatement(Statements.MethodInvoke(typeof(Console), "WriteLine", Operators.ObjectValue("Hello World!!!")))
                    .AddStatement(Statements.MethodInvoke(typeof(Console), "ReadLine"))
                )
                .ToCompile()
                .Executable()
                .SaveAs(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "ConsoleApp1.exe"));
        }
    }
}
