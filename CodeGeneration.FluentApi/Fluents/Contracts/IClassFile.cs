using System;
using System.CodeDom;
using System.Linq.Expressions;
using System.Reflection;

namespace CodeGeneration.FluentApi.Fluents.Contracts
{
    public interface IClassFile
    {
        IClassFile Name(string name);

        IClassFile InNamespace(string nameSpace);

        IClassFile AddImport(Type type);
        IClassFile AddImport(string typeName);

        IClassFile AddBaseType(Type type);
        IClassFile AddBaseType(string typeName);

        IClassFile AddCustomAttribute(string name);
        IClassFile AddCustomAttribute(string name, IAttributeParameter[] parameters);

        IClassFile AddAttribute(TypeAttributes attributes);

        IClassFile AddConstructor(IConstructor contructor);

        IClassFile AddField(IField field);
        IClassFile AddField(string name, string typeName);
        IClassFile AddField(string name, Type type);

        IClassFile AddProperty(IProperty property);
        IClassFile AddProperty(MemberAttributes modifiers, string propertyName, Type propertyType);

        IClassFile AddMethod(IMethod method);
        IClassFile AddMethod(MemberAttributes modifiers, string name, Type type);
        IClassFile AddMethod(MemberAttributes modifiers, string name, string typeName);
        IClassFile AddMethod(MemberAttributes modifiers, string name, Type type, IParameter[] parameters);
        IClassFile AddMethod(MemberAttributes modifiers, string name, string typeName, IParameter[] parameters);
        IClassFile AddVoidMethod(MemberAttributes modifiers, string name, IParameter[] parameters);
        
        void SaveTo(string path);

        ICompiler ToCompile();
    }
}
