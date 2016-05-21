using System;
using System.CodeDom;

namespace CodeGeneration.FluentApi.Snippets
{
    public static class Operators
    {
        public static CodeTypeOfExpression TypeOf(Type type)
        {
            return new CodeTypeOfExpression(new CodeTypeReference(type.FullName));
        }

        public static CodePrimitiveExpression ObjectValue(object value)
        {
            return new CodePrimitiveExpression(value);
        }
    }
}
