using System;
using CodeGeneration.FluentApi.Fluents.Contracts;

namespace CodeGeneration.FluentApi.DTO.Members
{
    public class Import : IImport
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

        public IImport ImportStatement(Type type)
        {
            Type = type;
            return this;
        }

        public IImport ImportStatement(string typeName)
        {
            TypeName = typeName;
            return this;
        }

        #endregion

        
    }
}
