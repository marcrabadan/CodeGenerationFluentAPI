
using System;
using System.CodeDom;
using CodeGeneration.FluentApi.DTO.Collections;
using CodeGeneration.FluentApi.Fluents.Contracts;

namespace CodeGeneration.FluentApi.DTO.Members
{
    public class Property : IProperty
    {
        #region .: Variables :. 

        private readonly CustomAttributeCollection _customAttributes;

        #endregion

        #region .: Constructor :.

        public Property()
        {
            _customAttributes = new CustomAttributeCollection();
        }

        #endregion

        #region .: Public Property :.

        public string PropertyName { get; set; }
        public Type PropertyType { get; set; }
        public bool PropertyHasGet { get; set; }
        public bool PropertyHasSet { get; set; }
        public MemberAttributes PropertyAttributes { get; set; }
        public CustomAttributeCollection PropertyCustomAttributes { get; set; }
       
        #endregion

        #region .: Public Methods :.

        public IProperty Name(string name)
        {
            PropertyName = name;
            return this;
        }

        public IProperty Type(Type type)
        {
            PropertyType = type;
            return this;
        }

        public IProperty HasGet()
        {
            PropertyHasGet = true;
            return this;
        }

        public IProperty HasSet()
        {
            PropertyHasSet = true;
            return this;
        }

        public IProperty CustomAttribute(string i)
        {
            throw new NotImplementedException();
        }

        public IProperty Attributes(MemberAttributes attributes)
        {
            PropertyAttributes = attributes;
            return this;
        }

        #endregion

    }
}
