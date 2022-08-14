using Dova.Common;
using Dova.Tools.DefinitionGenerator.Common;
using Dova.Tools.DefinitionGenerator.Models;

namespace Dova.Tools.DefinitionGenerator.Builders;

internal class MethodsBuilder : AbstractBuilder
{
    public override IEnumerable<string> Build(FileInfo outputFile, ClassDefinitionModel model, int tabs = 0)
    {
        var filteredMethods = DefinitionCleaner
            .GetMethodsToGenerate(model.ClassDetailsModel.ClassName, model.MethodModels)
            .ToList();
        
        for (var index = 0; index < filteredMethods.Count; ++index)
        {
            yield return AppendLine("");
            
            var method = filteredMethods[index];
            
            var modifierPrefix = method.IsStatic
                ? "static "
                : string.Empty;
            
            var combinedParameters = DefinitionCleaner.GetCombinedParameters(method.ParameterModels);
        
            var methodModifier = model.ClassDetailsModel.IsInterface
                ? string.Empty
                : "public ";
        
            var returnType = DefinitionCleaner.CleanClass(method.ReturnType);
            var cleanedMethodName = DefinitionCleaner.GetMethodName(method.Name);
            
            yield return AppendLine($"[{nameof(JniSignatureAttribute)}(\"{method.Signature}\", \"{method.Modifiers}\")]", tabs);
            yield return AppendLine($"{methodModifier}{modifierPrefix}{returnType} {cleanedMethodName}({combinedParameters})", tabs);
            yield return AppendLine("{", tabs);
            
            var staticMethodPrefix = method.IsStatic
                ? "Static"
                : "";
            
            var targetObjPtr = method.IsStatic
                ? Constants.ClassRefPtrStr
                : nameof(JavaObject.CurrentRefPtr);
            
            var returnTypeMethodPostfix = DefinitionCleaner.GetReturnTypeMethodPostfix(returnType);
            var combinedParameterNames = DefinitionCleaner.GetCombinedParameterNames(method.ParameterModels);
            
            if (!string.IsNullOrWhiteSpace(combinedParameterNames))
            {
                combinedParameterNames = ", " + combinedParameterNames;
            }
            
            var methodCallback = $"DovaVM.Runtime.Call{staticMethodPrefix}{returnTypeMethodPostfix}MethodA({targetObjPtr}, {Constants.MethodPtrsStr}[{index}]{combinedParameterNames});";
        
            if (returnTypeMethodPostfix.ToLower().Equals("void"))
            {
                yield return AppendLine(methodCallback, tabs + 1);
            }
            else
            {
                yield return AppendLine($"var ret = {methodCallback}", tabs + 1);
        
                var returnString = DefinitionCleaner.GetReturnString(model, method, returnType);
                
                yield return AppendLine(returnString, tabs + 1);
            }
            
            yield return AppendLine("}", tabs);
        }
    }
}