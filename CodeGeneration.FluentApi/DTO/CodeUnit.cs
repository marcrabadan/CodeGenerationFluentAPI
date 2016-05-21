using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.IO;
using System.Linq;
using CodeGeneration.FluentApi.DTO.Members;
using CodeGeneration.FluentApi.DTO.Collections;
using CodeGeneration.FluentApi.Enums;
using CodeGeneration.FluentApi.Extensions;
using CodeGeneration.FluentApi.Snippets;

namespace CodeGeneration.FluentApi.DTO
{
    public class CodeUnit
    {
        #region .: Private fields :.

        private readonly CodeDomProvider _codeDomProvider;
        private readonly CompilerParameters _options;

        #endregion

        #region .: Constructor :.

        public CodeUnit(CodeDomProvider codeDomProvider)
        {
            _codeDomProvider = codeDomProvider;
        }

        public CodeUnit(CodeDomProvider codeDomProvider, CompilerParameters options)
        {
            _codeDomProvider = codeDomProvider;
            _options = options;
        }

        #endregion

        #region .: Properties :.

        public ClassFile ClassFile { get; set; }
        public string SaveAs { get; set; }

        #endregion

        #region .: Public Properties :.

        public string SaveCSharpFile()
        {
            try
            {
                if (ClassFile != null)
                {
                    string path = string.Empty;
                    CodeCompileUnit compileUnit = GenerateCompileUnit(ClassFile);

                    using (StringWriter stringWriter = new StringWriter())
                    {
                        _codeDomProvider.GenerateCodeFromCompileUnit(compileUnit, stringWriter, null);
                        path = SaveFile(stringWriter.ToString(), SaveAs, string.Format("{0}.cs", ClassFile.ClassName));
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError("Error al generar el archivo: ", ex.Message);
            }
            finally
            {
                _codeDomProvider.Dispose();
            }
            return string.Empty;
        }

        public string SaveExeFile()
        {
            try
            {
                if (ClassFile != null)
                {
                    string path = string.Empty;
                    CodeCompileUnit compileUnit = GenerateCompileUnit(ClassFile);
                    var result = _codeDomProvider.CompileAssemblyFromDom(_options, compileUnit);
                    if(result.Errors.Count > 0)
                    {
                        throw new ArgumentException("No se ha podido compilar:", string.Join("\n", result.Errors.OfType<CompilerError>().Select(c => c.ErrorText).ToArray()));
                    }
                    else
                    {
                        return result.PathToAssembly;
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError("Error al generar el archivo: ", ex.Message);
                throw;
            }
            finally
            {
                _codeDomProvider.Dispose();
            }
            return string.Empty;
        }

        #endregion

        #region .: Private Properties :.

        private CodeCompileUnit GenerateCompileUnit(ClassFile codeFile)
        {
            CodeCompileUnit compileUnit = new CodeCompileUnit();

            CodeNamespace codeNamespace = new CodeNamespace(ClassFile.Namespace);

            compileUnit.Namespaces.Add(codeNamespace);

            codeNamespace.Imports.AddRange(ClassFile.Imports.ToCollection());

            //Class

            CodeTypeDeclaration typeDefinition = TypeDeclaration(ClassFile);

            //Fields

            CodeTypeMemberCollection fieldCollection = FieldCollection(ClassFile.Fields);

            //Properties

            CodeTypeMemberCollection propertyCollection = PropertyCollection(ClassFile.Properties);

            //Methods

            CodeTypeMemberCollection methodCollection = MethodCollection(ClassFile.Methods);

            typeDefinition.Members.AddRange(fieldCollection);

            typeDefinition.Members.AddRange(propertyCollection);

            typeDefinition.Members.AddRange(methodCollection);

            //Adding constructors
            if (codeFile.Constructors != null)
            {
                foreach (Constructor constructor in codeFile.Constructors)
                {
                    typeDefinition.Members.Add(ConstructorDeclaration(constructor));
                }
            }            

            codeNamespace.Types.Add(typeDefinition);

            return compileUnit;
        }

        private CodeTypeDeclaration TypeDeclaration(ClassFile classFile)
        {
            CodeTypeDeclaration typeDefinition = new CodeTypeDeclaration();
            typeDefinition.Name = classFile.ClassName;
            typeDefinition.IsClass = classFile.CodeFileType == CodeFileType.Class;
            typeDefinition.IsPartial = classFile.CodeFileType == CodeFileType.PartialClass;
            typeDefinition.IsInterface = classFile.CodeFileType == CodeFileType.Interface;

            if (classFile.BaseTypes != null && classFile.BaseTypes.Any())
            {
                typeDefinition.BaseTypes.AddRange(classFile.BaseTypes.ToCollection());
            }

            typeDefinition.TypeAttributes = classFile.Attributes;

            if (classFile.CustomAttributes != null && classFile.CustomAttributes.Any())
            {
                typeDefinition.CustomAttributes.AddRange(classFile.CustomAttributes.ToCollection());
            }

            return typeDefinition;
        }

        private CodeConstructor ConstructorDeclaration(Constructor constructor)
        {
            CodeConstructor codeConstructor = new CodeConstructor();
            codeConstructor.Attributes = constructor.ConstructorAttributes;

            if (constructor.ConstructorCustomAttributes != null && constructor.ConstructorCustomAttributes.Any())
                codeConstructor.CustomAttributes.AddRange(constructor.ConstructorCustomAttributes.ToCollection());

            if (constructor.ConstructorParameters != null && constructor.ConstructorParameters.Any())
                codeConstructor.Parameters.AddRange(constructor.ConstructorParameters.ToCollection());

            if (constructor.ConstructorStatements != null && constructor.ConstructorStatements.Count > 0)
                codeConstructor.Statements.AddRange(constructor.ConstructorStatements.ToArray());

            return codeConstructor;
        }

        private CodeTypeMemberCollection FieldCollection(FieldCollection fieldCollection)
        {
            CodeTypeMemberCollection memberCollection = new CodeTypeMemberCollection();
            if (fieldCollection != null && fieldCollection.Any())
            {
                foreach (var field in fieldCollection)
                {
                    memberCollection.Add(field.FieldType == null
                        ? BasicStatements.Field(field.FieldReadOnly, field.FieldTypeName, field.FieldName)
                        : BasicStatements.Field(field.FieldReadOnly, field.FieldType.ToCSharpStringFormat(), field.FieldName));
                }
            }
            return memberCollection;
        }
        
        private CodeTypeMemberCollection PropertyCollection(PropertyCollection propertyCollection)
        {
            CodeTypeMemberCollection memberCollection = new CodeTypeMemberCollection();
            if (propertyCollection != null && propertyCollection.Any())
            {
                foreach (var property in propertyCollection)
                {
                    memberCollection.Add(BasicStatements.Property(property.PropertyName, property.PropertyType, property.PropertyAttributes, property.PropertyCustomAttributes, property.PropertyHasGet, property.PropertyHasSet));
                }
            }
            return memberCollection;
        }
        
        private CodeTypeMemberCollection MethodCollection(MethodCollection methodCollection)
        {
            CodeTypeMemberCollection memberCollection = new CodeTypeMemberCollection();
            if (methodCollection != null && methodCollection.Any())
            {
                foreach (var method in methodCollection)
                {
                    memberCollection.Add(BasicStatements.Method(method.Name, method.Attributes, method.CustomAttributes, method.ReturnType, method.ReturnTypeName, method.IsVoid, method.Parameters, method.Statements));
                }
            }
            return memberCollection;
        }

        private string SaveFile(string content, string outputPath, string fileName)
        {
            try
            {
                using (Stream stream = File.Open(Path.Combine(outputPath, fileName), FileMode.Create))
                using(StreamWriter streamWriter = new StreamWriter(stream))
                {
                    streamWriter.Write(content);
                }
                return Path.Combine(outputPath, fileName);
            }
            catch (Exception ex)
            {
                Trace.TraceError("Error al guardar el archivo: ", ex.Message);
            }
            return string.Empty;
        }

        #endregion
    }
}
