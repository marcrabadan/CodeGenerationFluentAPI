using System;
using System.CodeDom;
using System.Reflection;

namespace CodeGeneration.FluentApi.Fluents.Contracts
{
    public interface IInterfaceFile
    {
        IInterfaceFile Name(string name);

        IInterfaceFile InNamespace(string nameSpace);

        IInterfaceFile AddImport(Type type);
        IInterfaceFile AddImport(string typeName);

        IInterfaceFile AddAttribute(TypeAttributes attributes);

        IInterfaceFile AddMethod(IMethod method);
        IInterfaceFile AddMethod(MemberAttributes modifiers, string name, Type type);
        IInterfaceFile AddMethod(MemberAttributes modifiers, string name, string typeName);
        IInterfaceFile AddMethod(MemberAttributes modifiers, string name, Type type, IParameter[] parameters);
        IInterfaceFile AddMethod(MemberAttributes modifiers, string name, string typeName, IParameter[] parameters);
        IInterfaceFile AddVoidMethod(MemberAttributes modifiers, string name, IParameter[] parameters);

        void SaveTo(string path);
    }
}
