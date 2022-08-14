namespace Dova.Tools.DefinitionGenerator.Models;

internal class ClassDetailsDefinitionModel
{
    public string PackageName { get; set; }
    public string ClassName { get; set; }
    public bool IsEnum { get; set; }
    public string Modifiers { get; set; }
    public IReadOnlyList<TypeParameterModel> TypeParameterModels { get; set; } = new List<TypeParameterModel>();
    public string Signature { get; set; }
    public bool IsInterface { get; set; }
    public bool IsAbstract { get; set; }
}