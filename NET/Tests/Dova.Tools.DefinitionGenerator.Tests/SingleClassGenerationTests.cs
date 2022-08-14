using System.IO;
using Dova.Tools.DefinitionGenerator.Core;
using Xunit;

namespace Dova.Tools.Tests;

public class SingleClassGenerationTests
{
    [Theory]
    [InlineData("/home/seia/dova_generated/jdk.compiler/com/sun/tools/javac/api/JavacTool.class.dova")]
    public void Should_generate_valid_CSharp_class(string definitionFilePath)
    {
        var config = new ClassGeneratorConfig
        {
            OutputDirectoryPath = "/tmp",
            TempDirPath = "/tmp",
        };

        var file = new FileInfo(definitionFilePath);
        
        ClassGenerator.ProcessDefinition(config, file);
    }
}