using System.CodeDom;

namespace CodeGeneration.FluentApi.Snippets
{
    public static class Expressions
    {
        public static CodeBinaryOperatorExpression CompareExpression(string variable, CodeBinaryOperatorType operatorType, object value)
        {
            return new CodeBinaryOperatorExpression
            {
                Left = new CodeVariableReferenceExpression(variable),
                Operator = operatorType,
                Right = new CodePrimitiveExpression(value)
            };
        }

        public static CodeSnippetExpression SnippetExpression(string script)
        {
            return new CodeSnippetExpression(script);
        }

        public static CodeFieldReferenceExpression FieldReferenceExpression(string variableName, string propertyName)
        {
            return new CodeFieldReferenceExpression(new CodeVariableReferenceExpression(variableName), propertyName);
        }
    }
}
