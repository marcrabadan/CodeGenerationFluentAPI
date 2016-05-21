using System;
using System.CodeDom;

namespace CodeGeneration.FluentApi.Fluents.Contracts
{
    public interface IField
    {
        IField FieldStatement(string name, Type type);
        IField FieldStatement(string name, string typeName);
        IField WithOptions(MemberAttributes modifiers);
        IField WithOptions(bool readOnly);
        IField SetValue(bool isConstant, object value);
        IField AddCustomAttribute(ICustomAttribute customAttribute);
        IField AddCustomAttribute(string name, IAttributeParameter parameter);
    }
}
