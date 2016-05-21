using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.CSharp;
using CodeGeneration.FluentApi.DTO.Collections;
using CodeGeneration.FluentApi.DTO.Members;
using CodeGeneration.FluentApi.Enums;
using CodeGeneration.FluentApi.Fluents;
using CodeGeneration.FluentApi.Fluents.Contracts;
using CodeGeneration.FluentApi.Fluents.Members;
using CodeGeneration.FluentApi.Snippets;
using CodeGeneration.FluentApi.Translator;
using CodeGeneration.FluentApi.Extensions;

namespace CodeGeneration.FluentApi.DTO
{
    public class Mapping : IMapping
    {
        #region .: Variables :. 

        private readonly List<KeyValuePair<Type, KeyValuePair<PropertyInfo, PropertyInfo>>> _mappingDictionary;
        private readonly List<KeyValuePair<Type, KeyValuePair<string, CodeStatement>>> _customMappingDictionary;
        private readonly List<KeyValuePair<Type, Type>> _resolveList;
        

        #endregion

        #region .: Constructor :.

        public Mapping()
        {
            _mappingDictionary = new List<KeyValuePair<Type, KeyValuePair<PropertyInfo, PropertyInfo>>>();
            _customMappingDictionary = new List<KeyValuePair<Type, KeyValuePair<string, CodeStatement>>>();
            _resolveList = new List<KeyValuePair<Type, Type>>();

            Imports = new ImportCollection();
            Fields = new FieldCollection();
            Methods = new MethodCollection();

            Attributes = TypeAttributes.Public;
            CodeFileType = CodeFileType.Class;
        }

        #endregion

        #region .: Public Properties :.

        public string MappingName { get; set; }
        public Type MappingFrom { get; set; }
        public Type MappingTo { get; set; }
        public string FromVariableName { get; set; }
        public string ToVariableName { get; set; }
        public string Namespace { get; set; }
        public CodeFileType CodeFileType { get; set; } 
        public TypeAttributes Attributes { get; set; }
        public ImportCollection Imports { get; set; }
        public FieldCollection Fields { get; set; } 
        public MethodCollection Methods { get; set; }

        #endregion

        #region .: Public Methods :.

        public IMapping Name(string name)
        {
            MappingName = name;
            return this;
        }

        public IMapping Map<TFrom, TTo>()
        {
            MappingFrom = typeof(TFrom);
            MappingTo = typeof(TTo);
            ToVariableName = "to";
            FromVariableName = "from";

            if (!_mappingDictionary.Any(c => c.Key == MappingFrom)) LoadPropertiesToDictionary(MappingFrom, MappingTo);
            return this;
        }

        public IMapping Map(Type from, Type to) 
        {
            MappingFrom = from;
            MappingTo = to;
            ToVariableName = "to";
            FromVariableName = "from";

            if (!_mappingDictionary.Any(c => c.Key == MappingFrom)) LoadPropertiesToDictionary(MappingFrom, MappingTo);
            return this;
        }

        public IMapping ResolveMapCollection(Dictionary<Type, Type> collection)
        {
            if (collection.Any())
            {
                if (!_resolveList.Any(c => collection.ContainsKey(c.Key))) _resolveList.AddRange(collection);
                if (!_mappingDictionary.Any(c => collection.ContainsKey(c.Key)))
                {
                    foreach (var kvp in collection) LoadPropertiesToDictionary(kvp.Key, kvp.Value);
                }
            }
            return this;
        }

        public IMapping Map<TFrom, TTo>(string fromVariableName, string toVariableName)
        {
            MappingFrom = typeof(TFrom);
            MappingTo = typeof(TTo);
            ToVariableName = toVariableName;
            FromVariableName = fromVariableName;

            if (!_mappingDictionary.Any(c => c.Key == MappingFrom)) LoadPropertiesToDictionary(MappingFrom, MappingTo);
            return this;
        }

        public IMapping ResolveMap<TFrom, TTo>()
        {
            if (!_resolveList.Any(c => c.Key == typeof(TFrom))) _resolveList.Add(new KeyValuePair<Type, Type>(typeof(TFrom),typeof(TTo)));
            if (!_mappingDictionary.Any(c => c.Key == typeof(TFrom))) LoadPropertiesToDictionary(typeof(TFrom), typeof(TTo));
            return this;
        }
    
        public IMapping Ignore(Type type, string propertyName)
        {
            try
            {
                if (MappingFrom != null && MappingTo != null)
                {
                    if (!_mappingDictionary.Any(c => c.Key == MappingFrom)) LoadPropertiesToDictionary(MappingFrom, MappingTo);

                    var propertyInfoSelected = (MappingFrom == type) ? MappingFrom.GetProperty(propertyName) : MappingTo.GetProperty(propertyName);

                    var keyValuePairs = _mappingDictionary.Select(c => c.Value).ToList();

                    if (keyValuePairs.Any(c => c.Key == propertyInfoSelected || c.Value == propertyInfoSelected))
                    {
                        var indexToRemove = keyValuePairs.IndexOf(keyValuePairs.FirstOrDefault(c => c.Key == propertyInfoSelected));
                        _mappingDictionary.RemoveAt(indexToRemove);
                    }
                }
                else
                {
                    throw new InvalidOperationException("you don't specificated map with Map method");
                }
            }
            catch (Exception ex)
            {
                Trace.TraceInformation("Error at Ignore(): {0}", ex.Message);
            }
            return this;
        }

        public IMapping IgnoreCollection(Dictionary<Type, string> properties)
        {
            try
            {
                if (MappingFrom != null && MappingTo != null && properties.Any())
                {
                    if (!_mappingDictionary.Any(c => c.Key == MappingFrom)) LoadPropertiesToDictionary(MappingFrom, MappingTo);
                    
                    var keyValuePairs = _mappingDictionary.Select(c => c.Value).ToList();

                    PropertyInfo propertyInfoSelected;
                    int indexToRemove;

                    foreach (var kvp in properties)
                    {
                        propertyInfoSelected = (MappingFrom == kvp.Key) ? MappingFrom.GetProperty(kvp.Value) : MappingTo.GetProperty(kvp.Value);
                        if (keyValuePairs.Any(c => c.Key == propertyInfoSelected || c.Value == propertyInfoSelected))
                        {
                            indexToRemove = keyValuePairs.IndexOf(keyValuePairs.FirstOrDefault(c => c.Key == propertyInfoSelected));
                            _mappingDictionary.RemoveAt(indexToRemove);
                        }
                    }
                }
                else
                {
                    throw new InvalidOperationException("you don't specificated map with Map method");
                }
            }
            catch (Exception ex)
            {
                Trace.TraceInformation("Error at IgnoreCollection(): {0}", ex.Message);
            }
            return this;
        }

        public IMapping AddImport(Type type)
        {
            Imports.Add((Import)MemberFluentFactory.InitImport().ImportStatement(type));
            return this;
        }

        public IMapping AddImport(string typeName)
        {
            Imports.Add((Import)MemberFluentFactory.InitImport().ImportStatement(typeName));
            return this;
        }
        
        public IMapping AddProperty<TFrom, TTo>(Expression<Func<TFrom, object>> fromProperty, Expression<Func<TTo, object>> toProperty) 
        {
            try
            {
                if (MappingFrom != null && MappingTo != null)
                {
                    if (!_mappingDictionary.Any(c => c.Key == MappingFrom)) LoadPropertiesToDictionary(MappingFrom, MappingTo);

                    var fromPropertyNameSelected = fromProperty.Body.ToString().Split('.')[1];
                    
                    var getProperties = ResolvePropertySelector<TTo>(toProperty);
                    if (getProperties != null)
                    {
                        _customMappingDictionary.Add(KeyValuePair(MappingFrom, KeyValuePair(string.Format("{0}.{1}", FromVariableName, fromPropertyNameSelected), Statements.AssignStatement(string.Format("{0}.{1}", ToVariableName, fromPropertyNameSelected), getProperties))));
                    }
                }
                else
                {
                    throw new InvalidOperationException("you don't specificated map with Map method");
                }
            }
            catch (Exception ex)
            {
                Trace.TraceInformation("Error at AddProperty<{0},{1}>(): {2}", typeof(TFrom).Name, typeof(TTo).Name, ex.Message);
            }
            return this;
        }
        
        public IMapping Ignore<T>(Expression<Func<T, object>> property)
        {
            try
            {
                if (MappingFrom != null && MappingTo != null)
                {
                    if (!_mappingDictionary.Any(c => c.Key == MappingFrom)) LoadPropertiesToDictionary(MappingFrom, MappingTo);

                    var cleanSelector = ResolvePropertySelector(property);

                    var propertyNameSelected = cleanSelector.Split('.')[1];
                    var propertyInfoSelected = (MappingFrom == property.Parameters[0].Type) ? MappingFrom.GetProperty(propertyNameSelected) : MappingTo.GetProperty(propertyNameSelected);

                    var keyValuePairs = _mappingDictionary.Select(c => c.Value).ToList();
                    
                    if (keyValuePairs.Any(c => c.Key == propertyInfoSelected || c.Value == propertyInfoSelected))
                    {
                        var indexToRemove = keyValuePairs.IndexOf(keyValuePairs.FirstOrDefault(c => c.Key == propertyInfoSelected));
                        _mappingDictionary.RemoveAt(indexToRemove);
                    }
                }
                else
                {
                    throw new InvalidOperationException("you don't specificated map with Map method");
                }
            }
            catch (Exception ex)
            {
                Trace.TraceInformation("Error at Ignore<{0}>(): {1}", typeof(T).Name, ex.Message);
            }
            return this;
        }

        public IMapping InNamespace(string nameSpace)
        {
            Namespace = nameSpace;
            return this;
        }

        public void SaveTo(string path)
        {
            try
            {
                IEnumerable<KeyValuePair<Type, IEnumerable<CodeStatement>>> statements = MatchStatementsToMapping();

                Save(MappingFrom, MappingTo, statements.First(c => c.Key == MappingFrom).Value.ToList(), path);

                foreach (var resolve in _resolveList) 
                    Save(resolve.Key, resolve.Value, statements.First(c => c.Key == resolve.Key).Value.ToList(), path);
            }
            catch (Exception ex)
            {
                Trace.TraceInformation("Error at SaveTo(): {0}", ex.Message);
            }
        }

        #endregion

        #region .: Private Methods :.

        private IEnumerable<KeyValuePair<Type, IEnumerable<CodeStatement>>> MatchStatementsToMapping()
        {
            var typesToMap = _mappingDictionary.Distinct(c => c.Key).ToList();
            var typesToCustomMap = _customMappingDictionary.Distinct(c => c.Key).ToList();

            List<KeyValuePair<Type, IEnumerable<CodeStatement>>> statements = new List<KeyValuePair<Type, IEnumerable<CodeStatement>>>();

            if (typesToMap.Any())
            {
                foreach (KeyValuePair<Type, KeyValuePair<PropertyInfo, PropertyInfo>> kvp in typesToMap)
                {
                    statements.Add(KeyValuePair(kvp.Key, SaveMappingStatements(_mappingDictionary.Where(c => c.Key == kvp.Key).Select(c => c.Value))));
                }
            }

            if (typesToCustomMap.Any())
            {
                IEnumerable<CodeStatement> auxStatements;
                List<CodeStatement> statementList = new List<CodeStatement>();
                foreach (KeyValuePair<Type, KeyValuePair<string, CodeStatement>> kvp in typesToCustomMap)
                {
                    auxStatements = SaveCustomMappingStatements(_customMappingDictionary.Where(c => c.Key == kvp.Key).Select(c => c.Value));
                    if (!statements.Any(c => c.Key == kvp.Key)) statements.Add(KeyValuePair(kvp.Key, auxStatements));
                    else
                    {
                        statementList.AddRange(statements.First(c => c.Key == kvp.Key).Value);
                        statementList.AddRange(auxStatements);
                        statements[statements.IndexOf(statements.First(c => c.Key == kvp.Key))] = new KeyValuePair<Type, IEnumerable<CodeStatement>>(kvp.Key, statementList);
                    }
                }
            }

            return statements;
        }

        private void Save(Type from, Type to, List<CodeStatement> statements, string path)
        {
            if(MappingFrom == from)
                MappingName = (string.IsNullOrEmpty(MappingName)) ? string.Format("{0}To{1}", MappingFrom.Name, MappingTo.Name) : MappingName;
            else
                MappingName = BuildMappingName(from, to);

            List<CodeStatement> statementCollection = new List<CodeStatement>();

            IEnumerable<PropertyInfo> properties = from.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(c => !c.PropertyType.IsValueType && !c.PropertyType.IsPrimitive && !c.PropertyType.Assembly.FullName.StartsWith("mscorlib") || c.PropertyType.FullName.StartsWith("System.Collections"));
            Type typeA, typeB;
            PropertyInfo propertyFoundOnTo;

            foreach (var property in properties)
            {
                if (statements.OfType<CodeAssignStatement>().Any(c => ((CodeVariableReferenceExpression)c.Left).VariableName.Contains(property.Name)))
                {
                    var item = statements.OfType<CodeAssignStatement>().FirstOrDefault(c => ((CodeVariableReferenceExpression)c.Left).VariableName.Contains(property.Name));
                    if(item != null) statements.Remove(item);

                    propertyFoundOnTo = to.GetProperties(BindingFlags.Public | BindingFlags.Instance).First(c => c.Name == property.Name);
                    typeA = property.PropertyType.GetGenericArguments().Any() ? property.PropertyType.GetGenericArguments()[0] : property.PropertyType;
                    typeB = propertyFoundOnTo.PropertyType.GetGenericArguments().Any() ? propertyFoundOnTo.PropertyType.GetGenericArguments()[0] : propertyFoundOnTo.PropertyType;

                    statementCollection.Add(Statements.VariableDeclarationWithInstanceStatement(BuildMappingName(typeA, typeB), string.Format("map{0}", property.Name)));
                    statements.Add(Statements.AssignStatement(string.Format("{0}.{1}", ToVariableName, property.Name), string.Format("{0}.Map({1}.{2})", string.Format("map{0}", property.Name), FromVariableName, property.Name)));
                }
            }

            FluentFactory
                .InitClass()
                .Name(MappingName)
                .AddAttribute(Attributes)
                .AddImport("System")
                .AddImport(typeof(IEnumerable).Namespace)
                .AddImport(from.Namespace)
                .AddImport(to.Namespace)
                .AddMethod(MemberFluentFactory
                                .InitMethod()
                                .MethodStatement(MemberAttributes.Public | MemberAttributes.Final, "Map")
                                .WithType(to)
                                .AddParameter(FromVariableName, from, ParameterDirection.None)
                                .AddStatement(
                                    Statements.ConditionalStatement(
                                        Expressions.CompareExpression(
                                            FromVariableName, 
                                            CodeBinaryOperatorType.ValueEquality, 
                                            null), 
                                        new StatementCollection
                                        {
                                            Statements.ReturnStatement("")
                                        }, null))
                                .AddStatements(statementCollection)
                                .AddStatement(Statements.VariableDeclarationWithInstanceStatement(to, ToVariableName))
                                .AddStatements(statements)
                                .AddStatement(Statements.ReturnStatement(ToVariableName)))
                .AddMethod(MemberFluentFactory
                    .InitMethod()
                    .MethodStatement(MemberAttributes.Public | MemberAttributes.Final, "Map")
                    .WithType(typeof(IEnumerable<>).MakeGenericType(new[] { to }))
                    .AddParameter(FromVariableName + "Collection", typeof(IEnumerable<>).MakeGenericType(new[] { from }), ParameterDirection.None)
                    .AddStatement(Statements.ReturnStatement(Expressions.SnippetExpression(string.Format("{0}Collection.Select(Map)", FromVariableName)))))
                .SaveTo(path);
        }

        private string Replace(string str)
        {
            string removeChars = " ?&^$#@!()+-,:;<>’\'-_*";
            string result = str;

            foreach (char c in removeChars)
            {
                result = result.Replace(c.ToString(), string.Empty);
            }

            return result;
        }

        private void LoadPropertiesToDictionary(Type fromType, Type toType)
        {
            IEnumerable<KeyValuePair<PropertyInfo, PropertyInfo>> matchProperties = (from fromProperty in fromType.GetProperties(BindingFlags.Public | BindingFlags.Instance).ToList()
                                                                                    join toProperty in toType.GetProperties(BindingFlags.Public | BindingFlags.Instance).ToList() on fromProperty.Name equals toProperty.Name
                                                                                    select new KeyValuePair<PropertyInfo, PropertyInfo>(fromProperty, toProperty));
            
            foreach (KeyValuePair<PropertyInfo, PropertyInfo> kvp in matchProperties)
            {
                _mappingDictionary.Add(KeyValuePair(fromType, kvp));
            }
        }

        private string ResolvePropertySelector<T>(Expression<Func<T, object>> property)
        {
            string cleanExpression = property.Body.ToString();
            var parameterName = property.Parameters[0].Name;

            if (cleanExpression.Contains("Format"))
            {
                if (cleanExpression.Contains("Convert"))
                {
                    cleanExpression = cleanExpression.Replace("Convert", string.Empty).Trim();
                }
                cleanExpression = cleanExpression.Replace(parameterName, FromVariableName);
                return cleanExpression.Replace("Format", "string.Format");
            }

            if (cleanExpression.Contains("+"))
            {
                var propertiesArray = cleanExpression.Split('+').ToList();

                var getProperties = propertiesArray.Select(c =>
                {
                    cleanExpression = Replace(c).Trim();
                    if (cleanExpression.Contains("Convert"))
                    {
                        cleanExpression = cleanExpression.Replace("Convert", string.Empty).Trim();
                    }
                    return cleanExpression.Replace(parameterName, FromVariableName);
                }).ToList();
                return string.Join(" + ", getProperties.ToArray());
            }

            if (cleanExpression.Contains("Convert"))
            {
                cleanExpression = Replace(cleanExpression).Trim();
                if (cleanExpression.Contains("Convert"))
                {
                    cleanExpression = cleanExpression.Replace("Convert", string.Empty).Trim();
                }
                cleanExpression = cleanExpression.Replace(parameterName, FromVariableName);
            }
            return cleanExpression;
        }

        private string DetermineObjectType(string strNamespace)
        {
            if (strNamespace.Contains("Contracts.ServiceLibrary")) return "DTO";
            if (strNamespace.Contains("Library")) return "Entity";
            else return String.Empty;
        }

        private IEnumerable<CodeStatement> SaveMappingStatements(IEnumerable<KeyValuePair<PropertyInfo, PropertyInfo>> properties)
        {
            return
                properties.Select(c => 
                    (CodeStatement)Statements.AssignStatement(
                        string.Format("{0}.{1}", ToVariableName, c.Value.Name), 
                        string.Format("{0}.{1}", FromVariableName, c.Key.Name)
                     ));
        }

        private string BuildMappingName(Type typeA, Type typeB)
        {
            return string.Format("{0}{1}To{2}{3}", typeA.Name, DetermineObjectType(typeA.Namespace), typeB.Name, DetermineObjectType(typeB.Namespace));
        }

        private IEnumerable<CodeStatement> SaveCustomMappingStatements(IEnumerable<KeyValuePair<string, CodeStatement>> properties)
        {
            return properties.Select(c => c.Value);
        }
        
        private KeyValuePair<string, CodeStatement> KeyValuePair(string str, CodeStatement statement)
        {
            return new KeyValuePair<string, CodeStatement>(str, statement);
        }

        private KeyValuePair<PropertyInfo, PropertyInfo> KeyValuePair(PropertyInfo propertyInfoSrc, PropertyInfo propertyInfoDst)
        {
            return new KeyValuePair<PropertyInfo, PropertyInfo>(propertyInfoSrc, propertyInfoDst);
        }

        private KeyValuePair<Type, KeyValuePair<PropertyInfo, PropertyInfo>> KeyValuePair(Type type, KeyValuePair<PropertyInfo, PropertyInfo> kvp)
        {
            return new KeyValuePair<Type, KeyValuePair<PropertyInfo, PropertyInfo>>(type, kvp);
        }

        private KeyValuePair<Type, KeyValuePair<string, CodeStatement>> KeyValuePair(Type type, KeyValuePair<string, CodeStatement> kvp)
        {
            return new KeyValuePair<Type, KeyValuePair<string, CodeStatement>>(type, kvp);
        }

        private KeyValuePair<Type, IEnumerable<CodeStatement>> KeyValuePair(Type type, IEnumerable<CodeStatement> statements)
        {
            return new KeyValuePair<Type, IEnumerable<CodeStatement>>(type, statements);
        }

        #endregion

        
    }
}
