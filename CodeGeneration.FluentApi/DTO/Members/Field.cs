using System;
using System.CodeDom;
using CodeGeneration.FluentApi.DTO.Collections;
using CodeGeneration.FluentApi.Fluents.Contracts;
using CodeGeneration.FluentApi.Fluents.Members;

namespace CodeGeneration.FluentApi.DTO.Members
{
    public class Field : IField
    {
        #region .: Variables :. 



        #endregion

        #region .: Constructor :.

        public Field()
        {
            FieldCustomAttributes = new CustomAttributeCollection();
        }

        #endregion

        #region .: Public Properties :.

        public string FieldName { get; set; }
        public Type FieldType { get; set; }
        public string FieldTypeName { get; set; }
        public object FieldValue { get; set; }
        public bool FieldReadOnly { get; set; }
        public MemberAttributes FieldAttributes { get; set; }
        public CustomAttributeCollection FieldCustomAttributes { get; set; }

        #endregion

        #region .: Public Methods :.

        public IField FieldStatement(string name, Type type)
        {
            FieldName = name;
            FieldType = type;
            FieldAttributes = MemberAttributes.Private;
            FieldReadOnly = true;
            return this;
        }

        public IField FieldStatement(string name, string typeName)
        {
            FieldName = name;
            FieldTypeName = typeName;
            FieldAttributes = MemberAttributes.Private;
            FieldReadOnly = true;
            return this;
        }

        public IField WithOptions(MemberAttributes modifiers)
        {
            FieldAttributes = modifiers;
            return this;
        }

        public IField WithOptions(bool readOnly)
        {
            FieldReadOnly = readOnly;
            return this;
        }

        public IField SetValue(bool isConstant, object value)
        {
            FieldAttributes = MemberAttributes.Const;
            FieldValue = value;
            return this;
        }

        public IField AddCustomAttribute(ICustomAttribute customAttribute)
        {
            FieldCustomAttributes.Add((CustomAttribute)customAttribute);
            return this;
        }

        public IField AddCustomAttribute(string name, IAttributeParameter parameter)
        {
            FieldCustomAttributes.Add((CustomAttribute)MemberFluentFactory.InitCustomAttribute().Name(name).AddAttribute(parameter));
            return this;
        }

        #endregion

        
    }
}
