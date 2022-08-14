using Dova.Tools.DefinitionGenerator.Common;
using Dova.Tools.DefinitionGenerator.Models;

namespace Dova.Tools.DefinitionGenerator.Builders;

internal class InterfacesBuilder : AbstractBuilder
{
    public override IEnumerable<string> Build(FileInfo outputFile, ClassDefinitionModel model, int tabs = 0)
    {
        if (model.InterfaceModels.Count == 0)
        {
            yield break;
        }
        
        foreach (var interfaceModel in model.InterfaceModels)
        {
            var cleanedName = DefinitionCleaner.CleanClass(interfaceModel.Name);
            yield return AppendLine($", {cleanedName}", tabs + 1);
        }
    }
}