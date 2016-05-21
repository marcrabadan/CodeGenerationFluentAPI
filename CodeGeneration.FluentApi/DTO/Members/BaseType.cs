using System;
using CodeGeneration.FluentApi.Fluents.Contracts;

namespace CodeGeneration.FluentApi.DTO.Members
{
    public class BaseType : IBaseType
    {
        #region .: Variables :. 



        #endregion

        #region .: Constructor :.



        #endregion

        #region .: Public Properties :.

        public string TypeName { get; set; }
        public Type Type { get; set; }

        #endregion

        #region .: Public Methods :.

        public IBaseType BaseTypeStatement(Type type)
        {
            Type = type;
            return this;
        }

        public IBaseType BaseTypeStatement(string typeName)
        {
            TypeName = typeName;
            return this;
        }

        #endregion

        
    }
}
