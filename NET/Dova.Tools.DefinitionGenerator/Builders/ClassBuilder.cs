using Dova.Tools.DefinitionGenerator.Models;

namespace Dova.Tools.DefinitionGenerator.Builders;

internal class ClassBuilder : AbstractBuilder
{
    private static IEnumerable<IBuilder> Builders = new List<IBuilder>
    {
        new ClassSignatureBuilder(),
        new BaseClassBuilder(),
        new InterfacesBuilder(),
        new ClassBodyBuilder(),
    };
    
    public override IEnumerable<string> Build(FileInfo outputFile, ClassDefinitionModel model, int tabs = 0) => 
        Builders.SelectMany(builder => builder.Build(outputFile, model, tabs));
}