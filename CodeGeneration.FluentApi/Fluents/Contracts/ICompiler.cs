
namespace CodeGeneration.FluentApi.Fluents.Contracts
{
    public interface ICompiler
    {
        ICompiler Executable();
        void SaveAs(string savePath);
    }
}
