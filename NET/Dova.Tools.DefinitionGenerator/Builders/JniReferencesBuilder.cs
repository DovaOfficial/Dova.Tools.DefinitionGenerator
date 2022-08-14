using Dova.Tools.DefinitionGenerator.Common;
using Dova.Tools.DefinitionGenerator.Models;

namespace Dova.Tools.DefinitionGenerator.Builders;

internal class JniReferencesBuilder : AbstractBuilder
{
    public override IEnumerable<string> Build(FileInfo outputFile, ClassDefinitionModel model, int tabs = 0)
    {
        yield return AppendLine($"public new static CSharpSystem.IntPtr {Constants.ClassPtrStr} {{ get; }}", tabs);
        yield return AppendLine($"public new static CSharpSystem.IntPtr {Constants.ClassRefPtrStr} {{ get; }}", tabs);
        
        yield return AppendLine("");
        
        if (model.FieldModels.Count > 0)
        {
            yield return AppendLine($"private new static CSharpSystem.Collections.Generic.IList<IntPtr> {Constants.FieldPtrsStr} {{ get; }} = new CSharpSystem.Collections.Generic.List<IntPtr>();", tabs);
        }
        
        if (model.ConstructorModels.Count > 0)
        {
            yield return AppendLine($"private new static CSharpSystem.Collections.Generic.IList<IntPtr> {Constants.ConstructorPtrsStr} {{ get; }} = new CSharpSystem.Collections.Generic.List<IntPtr>();", tabs);
        }
        
        if (model.MethodModels.Count > 0)
        {
            yield return AppendLine($"private new static CSharpSystem.Collections.Generic.IList<IntPtr> {Constants.MethodPtrsStr} {{ get; }} = new CSharpSystem.Collections.Generic.List<IntPtr>();", tabs);
        }
        
        yield return AppendLine("");
        
        var constructorName = DefinitionCleaner.GetLastInnerPart(model.ClassDetailsModel.ClassName);
        
        yield return AppendLine($"static {constructorName}()", tabs);
        yield return AppendLine("{", tabs);
        
        yield return AppendLine($"{Constants.ClassPtrStr} = DovaVM.Runtime.FindClass(\"{model.ClassDetailsModel.Signature}\");", tabs + 1);
        yield return AppendLine($"{Constants.ClassRefPtrStr} = DovaVM.Runtime.NewGlobalRef({Constants.ClassPtrStr});", tabs + 1);
        
        foreach (var fieldModel in model.FieldModels)
        {
            var runtimeMethod = fieldModel.IsStatic
                ? "GetStaticFieldId"
                : "GetFieldId";
        
            yield return AppendLine($"{Constants.FieldPtrsStr}.Add(DovaVM.Runtime.{runtimeMethod}({Constants.ClassPtrStr}, \"{fieldModel.Name}\", \"{fieldModel.Signature}\"));", tabs + 1);
        }

        foreach (var constructorModel in model.ConstructorModels)
        {
            var runtimeMethod = constructorModel.IsStatic
                ? "GetStaticMethodId"
                : "GetMethodId";
        
            yield return AppendLine($"{Constants.ConstructorPtrsStr}.Add(DovaVM.Runtime.{runtimeMethod}({Constants.ClassPtrStr}, \"<init>\", \"{constructorModel.Signature}\"));", tabs + 1);
        }
        
        var filteredMethods = DefinitionCleaner
            .GetMethodsToGenerate(model.ClassDetailsModel.ClassName, model.MethodModels)
            .ToList();
        
        foreach (var fieldModel in filteredMethods)
        {
            var runtimeMethod = fieldModel.IsStatic
                ? "GetStaticMethodId"
                : "GetMethodId";
        
            yield return AppendLine($"{Constants.MethodPtrsStr}.Add(DovaVM.Runtime.{runtimeMethod}({Constants.ClassPtrStr}, \"{fieldModel.Name}\", \"{fieldModel.Signature}\"));", tabs + 1);
        }
        
        yield return AppendLine("}", tabs);
    }
}