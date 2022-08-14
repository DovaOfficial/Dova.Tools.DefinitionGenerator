using Dova.Common;
using Dova.Tools.DefinitionGenerator.Common;
using Dova.Tools.DefinitionGenerator.Models;

namespace Dova.Tools.DefinitionGenerator.Builders;

internal class PropertiesBuilder : AbstractBuilder
{
    public override IEnumerable<string> Build(FileInfo outputFile, ClassDefinitionModel model, int tabs = 0)
    {
        for (var index = 0; index < model.FieldModels.Count; ++index)
        {
            yield return AppendLine("");
            
            var field = model.FieldModels[index];
            
            var staticPrefix = field.IsStatic
                ? "static "
                : "";
            
            var staticMethodPrefix = field.IsStatic
                ? "Static"
                : "";
            
            var targetObjPtr = field.IsStatic
                ? Constants.ClassRefPtrStr
                : nameof(JavaObject.CurrentRefPtr);
            
            var returnType = DefinitionCleaner.CleanClass(field.ReturnType);
            var returnValue = DefinitionCleaner.GetReturnValue(returnType);
            var returnTypeMethodPostfix = DefinitionCleaner.GetReturnTypeMethodPostfix(returnType);
            var propertyName = DefinitionCleaner.GetPropertyName(field.Name);
            var returnString = DefinitionCleaner.GetReturnString(model, field, returnType);
            
            yield return AppendLine($"[{nameof(JniSignatureAttribute)}(\"{field.Signature}\", \"{field.Modifiers}\")]", tabs);
            yield return AppendLine($"public {staticPrefix}{returnType} {propertyName}", tabs);
            yield return AppendLine("{", tabs);
            yield return AppendLine("get", tabs + 1);
            yield return AppendLine("{", tabs + 1);
            yield return AppendLine($"var ret = DovaVM.Runtime.Get{staticMethodPrefix}{returnTypeMethodPostfix}Field({targetObjPtr}, {Constants.FieldPtrsStr}[{index}]);", tabs + 2);
            yield return AppendLine(returnString, tabs + 2);
            yield return AppendLine("}", tabs + 1);
            yield return AppendLine("set", tabs + 1);
            yield return AppendLine("{", tabs + 1);
            yield return AppendLine($"DovaVM.Runtime.Set{staticMethodPrefix}{returnTypeMethodPostfix}Field({targetObjPtr}, {Constants.FieldPtrsStr}[{index}], {returnValue});", tabs + 2);
            yield return AppendLine("}", tabs + 1);
            yield return AppendLine("}", tabs);
        }
    }
}