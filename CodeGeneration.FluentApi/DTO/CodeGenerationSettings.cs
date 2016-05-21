using System.Collections.Generic;
using CodeGeneration.FluentApi.Enums;

namespace CodeGeneration.FluentApi.DTO
{
    public class CodeGenerationSettings
    {
        public List<string> ReferencedAssemblies { get; set; }
        public string OutputPath { get; set; }
        public bool OutputTempPath { get; set; }
        public CodeOutputFormat OutputFormat { get; set; }
    }
}
