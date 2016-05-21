using CodeGeneration.FluentApi.Fluents.Contracts;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using CodeGeneration.FluentApi.DTO.Members;
using System.Collections.Generic;
using System.Linq;

namespace CodeGeneration.FluentApi.DTO
{
    public class Compiler : ICompiler
    {
        private readonly CompilerParameters _options;
        private readonly ClassFile _classFile;
        private readonly CodeDomProvider _codeDomProvider;

        public Compiler(ClassFile classFile)
        {
            _codeDomProvider = new CSharpCodeProvider();
            _options = new CompilerParameters();
            _classFile = classFile;
        }

        public ICompiler Executable()
        {
            _options.GenerateExecutable = true;
            _options.ReferencedAssemblies.AddRange(_classFile.Imports
                .Cast<Import>()
                .Select(c => c.Type == null ? c.TypeName + ".dll" : c.Type.Module.Name)
                .ToArray());
            _options.MainClass = string.Format("{0}.{1}", _classFile.Namespace, _classFile.ClassName);
            _options.GenerateInMemory = true;
            return this;
        }

        public void SaveAs(string savePath)
        {
            _options.OutputAssembly = savePath;
            CodeUnit codeUnit = new CodeUnit(_codeDomProvider, _options)
            {
                ClassFile = _classFile,
                SaveAs = savePath
            };
            codeUnit.SaveExeFile();
        }
    }
}
