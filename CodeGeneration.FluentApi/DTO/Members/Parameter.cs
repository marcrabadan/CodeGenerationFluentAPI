using System;
using CodeGeneration.FluentApi.Enums;
using CodeGeneration.FluentApi.Fluents.Contracts;

namespace CodeGeneration.FluentApi.DTO.Members
{
    public class Parameter : IParameter
    {
        #region .: Variables :. 



        #endregion

        #region .: Constructor :.



        #endregion

        #region .: Public Properties :.

        public string ParameterName { get; set; }
        public Type ParameterType { get; set; }
        public string ParameterTypeName { get; set; }
        public ParameterDirection ParameterDirection { get; set; }

        #endregion

        #region .: Public Methods :.

        public IParameter Param(string name, Type type, ParameterDirection direction)
        {
            ParameterName = name;
            ParameterType = type;
            ParameterDirection = direction;
            return this;
        }

        public IParameter Param(string name, string typeName, ParameterDirection direction)
        {
            ParameterName = name;
            ParameterTypeName = typeName;
            ParameterDirection = direction;
            return this;
        }

        #endregion
    }
}
