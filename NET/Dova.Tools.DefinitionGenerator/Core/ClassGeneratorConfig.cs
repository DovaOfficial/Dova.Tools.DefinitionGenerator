namespace Dova.Tools.DefinitionGenerator.Core;

internal class ClassGeneratorConfig
{
    public string OutputDirectoryPath { get; set; }
    public string TempDirPath { get; set; }
    public string JavaClassDefinitionGeneratorPath { get; set; }
    public string JavaModuleFinderPaths { get; set; }
    public bool ForceGenerateJavaDefinitions { get; set; }
}