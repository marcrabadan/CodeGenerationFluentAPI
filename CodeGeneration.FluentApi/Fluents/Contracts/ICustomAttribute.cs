using System;

namespace CodeGeneration.FluentApi.Fluents.Contracts
{
    public interface ICustomAttribute
    {
        ICustomAttribute Name(string name);
        ICustomAttribute AddAttribute(IAttributeParameter parameter);
        ICustomAttribute AddAttribute(string name, Type type, object value);
    }
}
