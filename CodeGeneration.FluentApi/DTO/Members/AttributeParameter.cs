using System;
using CodeGeneration.FluentApi.Fluents.Contracts;

namespace CodeGeneration.FluentApi.DTO.Members
{
    public class AttributeParameter : IAttributeParameter
    {
        #region .: Variables :. 
        


        #endregion

        #region .: Constructor :.



        #endregion

        #region .: Public Properties :.
        
        public string ArgumentName { get; set; }
        public Type ArgumentType { get; set; }
        public object ArgumentValue { get; set; }
        
        #endregion

        #region .: Public Methods :.

        public IAttributeParameter AttributeParam(string name, Type type, object value)
        {
            ArgumentName = name;
            ArgumentType = type;
            ArgumentValue = value;
            return this;
        }

        #endregion

        
    }
}
