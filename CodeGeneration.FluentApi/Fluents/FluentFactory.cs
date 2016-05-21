using CodeGeneration.FluentApi.DTO;
using CodeGeneration.FluentApi.Fluents.Contracts;

namespace CodeGeneration.FluentApi.Fluents
{
    public static class FluentFactory
    {
        public static IClassFile InitClass()
        {
            return new ClassFile();
        }

        public static IInterfaceFile InitInterface()
        {
            return new InterfaceFile();
        }

        public static IMapping InitMapping()
        {
            return new Mapping();
        }
    }
}
