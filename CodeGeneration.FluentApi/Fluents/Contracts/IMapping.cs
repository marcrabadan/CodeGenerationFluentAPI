using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace CodeGeneration.FluentApi.Fluents.Contracts
{
    public interface IMapping
    {
        IMapping Map<TFrom, TTo>();
        IMapping Map<TFrom, TTo>(string fromVariableName, string toVariableName);

        IMapping ResolveMap<TFrom, TTo>();

        IMapping AddImport(Type type);
        IMapping AddImport(string typeName);
        
        IMapping AddProperty<TFrom, TTo>(Expression<Func<TFrom, object>> fromProperty, Expression<Func<TTo, object>> toProperty);

        IMapping Ignore<T>(Expression<Func<T, object>> property);

        IMapping InNamespace(string nameSpace);

        void SaveTo(string path);

        IMapping Name(string name);

        IMapping Map(Type from, Type to);
        IMapping ResolveMapCollection(Dictionary<Type, Type> collection);
        IMapping Ignore(Type type, string propertyName);
        IMapping IgnoreCollection(Dictionary<Type, string> properties);
    }
}
