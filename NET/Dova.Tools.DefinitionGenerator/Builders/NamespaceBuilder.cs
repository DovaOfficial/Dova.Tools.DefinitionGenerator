using Dova.Tools.DefinitionGenerator.Common;
using Dova.Tools.DefinitionGenerator.Models;

namespace Dova.Tools.DefinitionGenerator.Builders;

internal class NamespaceBuilder : AbstractBuilder
{
    public override IEnumerable<string> Build(FileInfo outputFile, ClassDefinitionModel model, int tabs = 0)
    {
        yield return AppendLine("");

        var cleanedNamespace = DefinitionCleaner.CleanKeywords(model.ClassDetailsModel.PackageName);
        var fullNamespace = DefinitionCleaner.AppendNamespacePrefix(cleanedNamespace);

        yield return AppendLine($"namespace {fullNamespace};");
    }
}