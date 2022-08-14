namespace Dova.Tools.DefinitionGenerator.Models;

internal class ParameterDefinitionModel
{
    public string Type { get; set; }
    public string Name { get; set; }
    public IReadOnlyList<TypeParameterModel> TypeParameterModels { get; set; } = new List<TypeParameterModel>();
    public string Signature { get; set; }
}