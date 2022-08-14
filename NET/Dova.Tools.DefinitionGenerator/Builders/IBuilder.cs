using Dova.Tools.DefinitionGenerator.Models;

namespace Dova.Tools.DefinitionGenerator.Builders;

internal interface IBuilder
{
    IEnumerable<string> Build(FileInfo outputFile, ClassDefinitionModel model, int tabs = 0);
}