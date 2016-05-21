using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CodeGeneration.FluentApi.DTO.Collections;
using CodeGeneration.FluentApi.Extensions;
using CodeGeneration.FluentApi.DTO.Members;

namespace CodeGeneration.FluentApi.Snippets
{
    public static class Statements
    {
        public static CodeAssignStatement AssignStatement(string variableName, object value)
        {
            return new CodeAssignStatement(
                new CodeVariableReferenceExpression(variableName),
                new CodePrimitiveExpression(value));
        }

        public static CodeAssignStatement AssignStatement(string srcVariableName, string dstVariableName)
        {
            return new CodeAssignStatement(
                new CodeVariableReferenceExpression(srcVariableName),
                new CodeVariableReferenceExpression(dstVariableName));
        }

        public static CodeAssignStatement AssignStatement(CodeFieldReferenceExpression leftFieldReferenceExpression, CodeFieldReferenceExpression rightFieldReferenceExpression)
        {
            return new CodeAssignStatement
            {
                Left = leftFieldReferenceExpression,
                Right = rightFieldReferenceExpression
            };
        }

        public static CodeStatement[] MatchType(string variableNameA, Type typeA, string variableNameB, Type typeB)
        {
            List<CodeStatement>  codeStatements = new List<CodeStatement>();
            PropertyInfo[] propertiesFromTypeA = typeA.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            PropertyInfo[] propertiesFromTypeB = typeB.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            IEnumerable<PropertyInfo> matchProperties = propertiesFromTypeA.Where(c => c.CanWrite && propertiesFromTypeB.Any(x => x.Name == c.Name)).ToList();

            codeStatements.AddRange(
                matchProperties.Select(c => Statements.AssignStatement(
                    Expressions.FieldReferenceExpression(variableNameA, c.Name),
                    Expressions.FieldReferenceExpression(variableNameB, c.Name)
                    )).ToArray()
                );

            return codeStatements.ToArray();
        }

        public static CodeVariableDeclarationStatement VariableDeclarationWithOutInstanceStatement(Type type, string variableName, object value)
        {
            return new CodeVariableDeclarationStatement(type.ToCSharpStringFormat(), variableName, new CodePrimitiveExpression(value));
        }

        public static CodeVariableDeclarationStatement VariableDeclarationWithInstanceStatement(Type type, string variableName)
        {
            return new CodeVariableDeclarationStatement(type.ToCSharpStringFormat(), variableName, new CodeObjectCreateExpression(type.ToCSharpStringFormat()));
        }

        public static CodeVariableDeclarationStatement VariableDeclarationWithInstanceStatement(string stringType, string variableName)
        {
            return new CodeVariableDeclarationStatement(stringType, variableName, new CodeObjectCreateExpression(stringType));
        }

        public static CodeIterationStatement LoopStatement()
        {
            return null;
        }

        public static CodeConditionStatement ConditionalStatement(CodeExpression condition, StatementCollection trueStatement, StatementCollection falseStatement)
        {
            CodeConditionStatement conditionStatement = null;
            if (trueStatement != null && trueStatement.Count > 0)
            {
                if (falseStatement != null && falseStatement.Count > 0)
                    conditionStatement =  new CodeConditionStatement(condition, trueStatement.ToArray(), falseStatement.ToArray());
                else 
                    conditionStatement = new CodeConditionStatement(condition, trueStatement.ToArray());
            } 
            return conditionStatement;
        }

        public static CodeMethodReturnStatement ReturnStatement(string variableName)
        {
            if (string.IsNullOrEmpty(variableName)) return new CodeMethodReturnStatement(new CodePrimitiveExpression(null));
            return new CodeMethodReturnStatement(new CodeArgumentReferenceExpression(variableName));
        }

        public static CodeMethodReturnStatement ReturnStatement(CodeExpression expression)
        {
            return new CodeMethodReturnStatement(expression);
        }

        public static CodeThrowExceptionStatement ThrowExceptionStatement(Type exceptionType)
        {
            return new CodeThrowExceptionStatement(
                new CodeObjectCreateExpression(
                    new CodeTypeReference(exceptionType.ToCSharpStringFormat()), 
                    new CodeExpression[] { }));
        }

        public static CodeStatement MethodInvoke(Type type, string methodNameToInvoke)
        {
            return new CodeExpressionStatement(new CodeMethodInvokeExpression(new CodeTypeReferenceExpression(type), methodNameToInvoke));
        }

        public static CodeStatement MethodInvoke(Type type, string methodNameToInvoke, params CodeExpression[] expressions)
        {
            return new CodeExpressionStatement(new CodeMethodInvokeExpression(new CodeTypeReferenceExpression(type), methodNameToInvoke, expressions));
        }
    }
}
