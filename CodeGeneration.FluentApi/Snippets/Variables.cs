using System;
using System.CodeDom;
using System.Linq;

namespace CodeGeneration.FluentApi.Snippets
{
    public static class Variables
    {
        public static CodeVariableDeclarationStatement VariableWithInstanceDeclaration(Type referenceType, string variableName, object objectValue)
        {
            return new CodeVariableDeclarationStatement(referenceType, variableName, Operators.ObjectValue(objectValue));
        }

        public static CodeVariableDeclarationStatement VariableDeclaration(Type referenceType, string variableName, object objectValue)
        {
            return new CodeVariableDeclarationStatement(referenceType, variableName, Operators.ObjectValue(objectValue));
        }

        public static CodeTypeParameter GenericTypeDefinition(Type genericType, string genericTypeName, bool instance, string descriptionGenericType = null, Type[] baseTypes = null)
        {
            CodeTypeParameter genericTypeParameter = new CodeTypeParameter(genericTypeName);
            genericTypeParameter.Constraints.Add(new CodeTypeReference(genericType));
            genericTypeParameter.HasConstructorConstraint = instance;
            if (!string.IsNullOrEmpty(descriptionGenericType))
            {
                genericTypeParameter.CustomAttributes.Add(
                    new CodeAttributeDeclaration(
                        "System.ComponentModel.DescriptionAttribute",
                        new CodeAttributeArgument(
                            new CodePrimitiveExpression(genericTypeName)
                        )
                    )
                );
            }
            CodeTypeReference refType;
            foreach (var baseType in baseTypes)
            {
                if (baseType.GetGenericArguments().Any())
                {
                    refType = new CodeTypeReference(baseType);
                    refType.TypeArguments.AddRange(baseType.GetGenericArguments().Select(c => new CodeTypeReference(c)).ToArray());
                    genericTypeParameter.Constraints.Add(refType);
                }
                else
                {
                    genericTypeParameter.Constraints.Add(new CodeTypeReference(genericType));
                }
            }
            return genericTypeParameter;
        }

    }
}
