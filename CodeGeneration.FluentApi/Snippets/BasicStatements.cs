using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CodeGeneration.FluentApi.DTO.Collections;
using CodeGeneration.FluentApi.Extensions;

namespace CodeGeneration.FluentApi.Snippets
{
    public static class BasicStatements
    {
        public static CodeTypeDeclaration ClassDeclaration(string className, bool isPartial, TypeAttributes accessModifiers, Type[] baseTypes = null, Type[] parameterTypes = null)
        {
            CodeTypeDeclaration classDeclaration = new CodeTypeDeclaration(className);
            classDeclaration.IsClass = true;
            classDeclaration.IsPartial = isPartial;
            classDeclaration.TypeAttributes = accessModifiers;
            CodeTypeReference refType;
            if (baseTypes != null && baseTypes.Any())
            {
                foreach (var baseType in baseTypes)
                {
                    if (baseType.GetGenericArguments().Any())
                    {
                        refType = new CodeTypeReference(baseType);
                        refType.TypeArguments.AddRange(baseType.GetGenericArguments().Select(c => new CodeTypeReference(c)).ToArray());
                        classDeclaration.BaseTypes.Add(refType);
                    }
                    else
                    {
                        classDeclaration.BaseTypes.Add(new CodeTypeReference(baseType));
                    }
                }

                classDeclaration.BaseTypes.AddRange(baseTypes.Select(c => new CodeTypeReference(c)).ToArray());
            }
            if (parameterTypes != null && parameterTypes.Any())
            {
                foreach (var parameterType in parameterTypes)
                {
                    if (parameterType.GetGenericArguments().Any())
                    {
                        refType = new CodeTypeReference(parameterType);
                        refType.TypeArguments.AddRange(parameterType.GetGenericArguments().Select(c => new CodeTypeReference(c)).ToArray());
                        classDeclaration.BaseTypes.Add(refType);
                    }
                    else
                    {
                        classDeclaration.BaseTypes.Add(new CodeTypeReference(parameterType));
                    }
                }
            }
            return classDeclaration;
        }
        
        public static CodeTypeDeclaration EnumDeclaration(string enumName, IEnumerable<string> values)
        {
            CodeTypeDeclaration enumDeclaration = new CodeTypeDeclaration(enumName);
            enumDeclaration.IsEnum = true;

            foreach (var value in values)
            {
                enumDeclaration.Members.Add(new CodeMemberField(enumName, value));
            }

            return enumDeclaration;
        }

        public static CodeTypeDeclaration InterfaceDeclaration(string className, TypeAttributes accessModifiers, Type[] baseTypes = null)
        {
            CodeTypeDeclaration interfaceDeclaration = new CodeTypeDeclaration(className);
            interfaceDeclaration.IsInterface = true;
            interfaceDeclaration.TypeAttributes = accessModifiers;

            CodeTypeReference refType;
            if (baseTypes != null && baseTypes.Any())
            {
                foreach (var baseType in baseTypes)
                {
                    if (baseType.GetGenericArguments().Any())
                    {
                        refType = new CodeTypeReference(baseType);
                        refType.TypeArguments.AddRange(baseType.GetGenericArguments().Select(c => new CodeTypeReference(c)).ToArray());
                        interfaceDeclaration.BaseTypes.Add(refType);
                    }
                    else
                    {
                        interfaceDeclaration.BaseTypes.Add(new CodeTypeReference(baseType));
                    }
                }
            }
            return interfaceDeclaration;
        }

        public static CodeRegionDirective Region(CodeRegionMode mode, string regionName)
        {
            return new CodeRegionDirective(mode, regionName);
        }

        public static CodeMemberField Field(bool isReadOnly, string typeName, string name)
        {
            return new CodeMemberField(isReadOnly ? "readonly " + typeName : typeName, name);
        }

        public static CodeMemberProperty Property(string propertyName, Type type, MemberAttributes modifiers, CustomAttributeCollection customAttributes, bool hasGet, bool hasSet)
        {
            CodeMemberProperty property = new CodeMemberProperty
            {
                Name = propertyName,
                Type = new CodeTypeReference(type == null ? "" : type.ToCSharpStringFormat()),
                Attributes = modifiers,
                HasGet = hasGet,
                HasSet = hasSet,
                CustomAttributes = customAttributes.ToCollection()
            };
            return property;
        }

        public static CodeMemberMethod Method(string name, MemberAttributes attributes, CustomAttributeCollection customAttributeCollection, Type TypeToReturn, string TypeName, bool isVoid, ParameterCollection parameterCollection, StatementCollection statementCollection)
        {
            CodeTypeReference returnType = new CodeTypeReference("");
            if(TypeToReturn != null) returnType = new CodeTypeReference(TypeToReturn.ToCSharpStringFormat()); 
            if (!string.IsNullOrEmpty(TypeName)) returnType = new CodeTypeReference(TypeName);

            CodeMemberMethod property = new CodeMemberMethod
            {
                Name = name,
                Attributes = attributes,
                CustomAttributes = customAttributeCollection != null ? customAttributeCollection.ToCollection() : null,
                ReturnType = returnType
            };

            if (parameterCollection != null && parameterCollection.Any())
            {
                property.Parameters.AddRange(parameterCollection.ToCollection());                
            }

            if (statementCollection != null && statementCollection.Count > 0)
            {
                property.Statements.AddRange(statementCollection);
            }

            return property;
        }
    }
}
