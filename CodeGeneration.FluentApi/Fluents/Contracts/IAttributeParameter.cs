using System;

namespace CodeGeneration.FluentApi.Fluents.Contracts
{
    public interface IAttributeParameter
    {
        IAttributeParameter AttributeParam(string name, Type type, object value);
    }
}
