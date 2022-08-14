using System.Diagnostics;
using System.Text.Json;
using Dova.Tools.DefinitionGenerator.Models;
using Dova.Tools.DefinitionGenerator.Builders;

namespace Dova.Tools.DefinitionGenerator.Core;

internal static class ClassGenerator
{
    private static CoreBuilder Builder { get; } = new();
    
    public static void Run(ClassGeneratorConfig config)
    {
        if (config.ForceGenerateJavaDefinitions)
        {
            GenerateJavaClassDefinitions(config.JavaClassDefinitionGeneratorPath, config.TempDirPath, config.JavaModuleFinderPaths);
        }

        var definitionFiles = GetDefinitionFiles(config.TempDirPath);

        var parallelOptions = new ParallelOptions
        {
            MaxDegreeOfParallelism = 20
        };
        
        Parallel.ForEach(definitionFiles, parallelOptions, definitionFile => ProcessDefinition(config, definitionFile));
    }
    
    public static void GenerateJavaClassDefinitions(string classDefinitionGeneratorPath, string tempOutputPathFull, string javaModuleFinderPaths = "")
    {
        var process = new Process
        {
            StartInfo =
            {
                FileName = "java",
                Arguments = $"-jar {classDefinitionGeneratorPath} {tempOutputPathFull} {javaModuleFinderPaths}"
            }
        };

        process.ErrorDataReceived += (sender, e) => Console.WriteLine(sender.ToString());

        process.Start();
        process.WaitForExit();
    }

    public static IEnumerable<FileInfo> GetDefinitionFiles(string path)
    {
        var dir = new DirectoryInfo(path);

        var definitionFilesInDirectory = dir
            .GetFiles()
            .Where(fileInfo => fileInfo.Extension.Equals(".dova") && fileInfo.Length > 0)
            .ToList();

        foreach (var subDir in dir.GetDirectories())
        {
            var definitionFilesInSubDir = GetDefinitionFiles(subDir.FullName);
            definitionFilesInDirectory.AddRange(definitionFilesInSubDir);
        }

        return definitionFilesInDirectory;
    }
    
    public static void ProcessDefinition(ClassGeneratorConfig config, FileInfo definitionFile)
    {
        var javaClassDefinitionModel = ReadClassDefinition(definitionFile.FullName);

        var outputPathFull = definitionFile.FullName
            .Replace(config.TempDirPath, config.OutputDirectoryPath)
            .Replace(".class.dova", ".cs");
        
        GenerateClass(outputPathFull, javaClassDefinitionModel);
    }

    public static ClassDefinitionModel ReadClassDefinition(string path)
    {
        var fileContent = File.ReadAllText(path);
        var model = JsonSerializer.Deserialize<ClassDefinitionModel>(fileContent);
        
        return model ?? throw new ArgumentException("Cannot read definition file at: " + path);
    }
    
    public static void GenerateClass(string outputPathFull, ClassDefinitionModel model)
    {
        var outputFile = new FileInfo(outputPathFull);

        if (!Directory.Exists(outputFile.DirectoryName))
        {
            Directory.CreateDirectory(outputFile.DirectoryName);
        }

        if (outputFile.Exists)
        {
            outputFile.Delete();
        }

        var lines = Builder
            .Build(outputFile, model)
            .ToList();

        using (var writer = new StreamWriter(outputFile.FullName))
        {
            foreach (var line in lines)
            {
                writer.WriteLine(line);
            }
        }
    }
}