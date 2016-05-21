using CodeGeneration.FluentApi.DTO.Members;
using CodeGeneration.FluentApi.Fluents.Contracts;

namespace CodeGeneration.FluentApi.Fluents.Members
{
    public static class MemberFluentFactory
    {
        public static IProperty InitProperty()
        {
            return new Property();
        }

        public static IAttributeParameter InitAttributeParameter()
        {
            return new AttributeParameter();
        }

        public static ICustomAttribute InitCustomAttribute()
        {
            return new CustomAttribute();
        }

        public static IField InitField()
        {
            return new Field();
        }

        public static IBaseType InitBaseType()
        {
            return new BaseType();
        }

        public static IConstructor InitConstructor()
        {
            return new Constructor();
        }

        public static IParameter InitParameter()
        {
            return new Parameter();
        }

        public static IImport InitImport()
        {
            return new Import();
        }

        public static IMethod InitMethod()
        {
            return new Method();
        }
    }
}
