using Dova.Tools.DefinitionGenerator.Models;

namespace Dova.Tools.DefinitionGenerator.Builders;

internal class LicenceBuilder : AbstractBuilder
{
    public override IEnumerable<string> Build(FileInfo outputFile, ClassDefinitionModel model, int tabs = 0)
    {
        yield return AppendLine($"/*******************************************************************");
        yield return AppendLine($" *");
        yield return AppendLine($" * This file was automatically generated with:");
        yield return AppendLine($" * Dova.Tools.DefinitionGenerator (on {DateTime.Now:yyyy.MM.dd-hh:mm})");
        yield return AppendLine($" *");
        yield return AppendLine($" * For more information please visit:");
        yield return AppendLine($" * https://github.com/DovaOfficial");
        yield return AppendLine($" *");
        yield return AppendLine($" ********************************************************************/");
    }
}