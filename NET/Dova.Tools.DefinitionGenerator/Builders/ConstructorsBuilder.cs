using Dova.Common;
using Dova.Tools.DefinitionGenerator.Common;
using Dova.Tools.DefinitionGenerator.Models;

namespace Dova.Tools.DefinitionGenerator.Builders;

internal class ConstructorsBuilder : AbstractBuilder
{
    public override IEnumerable<string> Build(FileInfo outputFile, ClassDefinitionModel model, int tabs = 0)
    {
        if (model.ClassDetailsModel.IsInterface)
        {
            yield break;
        }

        var constructorName = DefinitionCleaner.GetLastInnerPart(model.ClassDetailsModel.ClassName);
        
        yield return AppendLine("");
        yield return AppendLine($"[{nameof(JniSignatureAttribute)}(\"({typeof(IntPtr).FullName})V\", \"public\")]", tabs);
        yield return AppendLine($"public {constructorName}(IntPtr currentRefPtr) : base(currentRefPtr)", tabs);
        yield return AppendLine("{", tabs);
        yield return AppendLine("}", tabs);
        
        for (var index = 0; index < model.ConstructorModels.Count; ++index)
        {
            yield return AppendLine("");
            
            var constructorModel = model.ConstructorModels[index];
            var combinedParameters = DefinitionCleaner.GetCombinedParameters(constructorModel.ParameterModels);
            var combinedParameterNames = DefinitionCleaner.GetCombinedParameterNames(constructorModel.ParameterModels);
        
            if (!string.IsNullOrWhiteSpace(combinedParameterNames))
            {
                combinedParameterNames = ", " + combinedParameterNames;
            }
        
            yield return AppendLine($"[{nameof(JniSignatureAttribute)}(\"{constructorModel.Signature}\", \"{constructorModel.Modifiers}\")]", tabs);
            yield return AppendLine($"public {constructorName}({combinedParameters}) : base(DovaVM.Runtime.NewObjectA({Constants.ClassPtrStr}, {Constants.ConstructorPtrsStr}[{index}]{combinedParameterNames}))", tabs);
            yield return AppendLine("{", tabs);
            yield return AppendLine("}", tabs);
        }
    }
}