using System;
using System.CodeDom;

namespace CodeGeneration.FluentApi.Fluents.Contracts
{
    public interface IProperty
    {
        IProperty Name(string name);
        IProperty Type(Type type);
        IProperty HasGet();
        IProperty HasSet();
        IProperty CustomAttribute(string i);
        IProperty Attributes(MemberAttributes attributes);
    }
}
