using Dova.Common;
using Dova.Tools.DefinitionGenerator.Common;
using Dova.Tools.DefinitionGenerator.Models;

namespace Dova.Tools.DefinitionGenerator.Builders;

internal class ClassSignatureBuilder : AbstractBuilder
{
    public override IEnumerable<string> Build(FileInfo outputFile, ClassDefinitionModel model, int tabs = 0)
    {
        yield return AppendLine("");
        
        var type = model.ClassDetailsModel.IsInterface
            ? "interface "
            : "class ";

        var className = DefinitionCleaner.GetLastInnerPart(model.ClassDetailsModel.ClassName);

        yield return AppendLine($"[{nameof(JniSignatureAttribute)}(\"{model.ClassDetailsModel.Signature}\", \"{model.ClassDetailsModel.Modifiers}\")]", tabs);
        yield return AppendLine($"public partial {type}{className}", tabs);
    }
}