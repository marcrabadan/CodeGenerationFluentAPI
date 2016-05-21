using System;
using CodeGeneration.FluentApi.DTO.Collections;
using CodeGeneration.FluentApi.Fluents.Contracts;
using CodeGeneration.FluentApi.Fluents.Members;

namespace CodeGeneration.FluentApi.DTO.Members
{
    public class CustomAttribute : ICustomAttribute
    {
        #region .: Variables :. 



        #endregion

        #region .: Constructor :.

        public CustomAttribute()
        {
            CustomAttributeArguments = new AttributeArgumentCollection();
        }

        #endregion

        #region .: Public Properties :.

        public string CustomAttributeName { get; set; }
        public AttributeArgumentCollection CustomAttributeArguments { get; set; }

        #endregion

        #region .: Public Methods :.

        public ICustomAttribute Name(string name)
        {
            CustomAttributeName = name;
            return this;
        }

        public ICustomAttribute AddAttribute(IAttributeParameter parameter)
        {
            CustomAttributeArguments.Add((AttributeParameter)parameter);
            return this;
        }

        public ICustomAttribute AddAttribute(string name, Type type, object value)
        {
            CustomAttributeArguments.Add((AttributeParameter)MemberFluentFactory.InitAttributeParameter().AttributeParam(name, type, value));
            return this;
        }

        #endregion

        
    }
}
