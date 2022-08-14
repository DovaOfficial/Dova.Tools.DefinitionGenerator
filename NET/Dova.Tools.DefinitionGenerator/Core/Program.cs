using Dova.Tools.DefinitionGenerator.Core;

var generatorConfig = new ClassGeneratorConfig
{
    OutputDirectoryPath = args[0], // full output path where to generate C# classes i.e.: /home/<user>/Dev/Dotnet/Dova/src/Dova.JDK
    TempDirPath = args[1], // path to temporary directory where definition files will be generated i.e.: /tmp
    JavaClassDefinitionGeneratorPath = args[2], // path to java class generating tool i.e.: /home/<user>/Dev/Dotnet/Dova/src/Tools/Dova.Tools.Java.ClassGenerator/build/libs/Dova.Tools.Java.ClassGenerator-0.1.jar
    JavaModuleFinderPaths = args.Length > 3 ? args[3] : string.Empty, // [Optional] full paths to java modules from which definition files should be 
    ForceGenerateJavaDefinitions = args.Length > 4 && bool.Parse(args[4]), // [Optional] forces call to generate java definitions using java tool from provided path
};

ClassGenerator.Run(generatorConfig);