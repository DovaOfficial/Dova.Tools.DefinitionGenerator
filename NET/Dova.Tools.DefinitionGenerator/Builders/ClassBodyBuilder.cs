using Dova.Tools.DefinitionGenerator.Models;

namespace Dova.Tools.DefinitionGenerator.Builders;

internal class ClassBodyBuilder : AbstractBuilder
{
    private static IEnumerable<IBuilder> Builders = new List<IBuilder>
    {
        new JniReferencesBuilder(),
        new PropertiesBuilder(),
        new ConstructorsBuilder(),
        new ExtraMethodsBuilder(),
        new MethodsBuilder(),
        new InnerClassesBuilder(),
    };
    
    public override IEnumerable<string> Build(FileInfo outputFile, ClassDefinitionModel model, int tabs = 0)
    {
        yield return AppendLine("{", tabs);

        foreach (var line in Builders.SelectMany(builder => builder.Build(outputFile, model, tabs + 1)))
        {
            yield return line;
        }
        
        yield return AppendLine("}", tabs);
    }
}