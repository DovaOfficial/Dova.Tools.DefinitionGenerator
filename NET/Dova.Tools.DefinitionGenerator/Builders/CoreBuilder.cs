using Dova.Tools.DefinitionGenerator.Models;

namespace Dova.Tools.DefinitionGenerator.Builders;

internal class CoreBuilder : AbstractBuilder
{
    private static IEnumerable<IBuilder> Builders = new List<IBuilder>
    {
        new LicenceBuilder(),
        new UsingsBuilder(),
        new NamespaceBuilder(),
        new ClassBuilder(),
    };
    
    public override IEnumerable<string> Build(FileInfo outputFile, ClassDefinitionModel model, int tabs = 0) => 
        Builders.SelectMany(builder => builder.Build(outputFile, model, tabs));
}