using Dova.Tools.DefinitionGenerator.Models;

namespace Dova.Tools.DefinitionGenerator.Builders;

internal class InnerClassesBuilder : AbstractBuilder
{
    private static ClassBuilder Builder { get; } = new();

    public override IEnumerable<string> Build(FileInfo outputFile, ClassDefinitionModel model, int tabs = 0) =>
        model.InnerClassModels.SelectMany(innerClassModel => Builder.Build(outputFile, innerClassModel, tabs));
}