using Dova.Common;
using Dova.Tools.DefinitionGenerator.Common;
using Dova.Tools.DefinitionGenerator.Models;

namespace Dova.Tools.DefinitionGenerator.Builders;

internal class ExtraMethodsBuilder : AbstractBuilder
{
    public override IEnumerable<string> Build(FileInfo outputFile, ClassDefinitionModel model, int tabs = 0)
    {
        if (model.ClassDetailsModel.IsInterface)
        {
            yield break;
        }
        
        yield return AppendLine("");
        yield return AppendLine($"public override string {nameof(JavaObject.GetJavaClassSignature)}() => \"{model.ClassDetailsModel.Signature}\";", tabs);
        yield return AppendLine($"public override IntPtr {nameof(JavaObject.GetJavaClassRaw)}() => {Constants.ClassPtrStr};", tabs);
        yield return AppendLine($"public override IntPtr {nameof(JavaObject.GetJavaClassRefRaw)}() => {Constants.ClassRefPtrStr};", tabs);
    }
}