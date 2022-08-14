namespace Dova.Tools.DefinitionGenerator.Models;

internal class ClassElementDefinitionModel
{
    public string Modifiers { get; set; }
    public string ReturnType { get; set; }
    public string Name { get; set; }
    public IReadOnlyList<ParameterDefinitionModel> ParameterModels { get; set; } = new List<ParameterDefinitionModel>();
    public IReadOnlyList<TypeParameterModel> TypeParameterModels { get; set; } = new List<TypeParameterModel>();
    public string Signature { get; set; }
    public bool IsStatic { get; set; }
    public bool HasParent { get; set; }
}