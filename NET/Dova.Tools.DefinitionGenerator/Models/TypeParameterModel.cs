namespace Dova.Tools.DefinitionGenerator.Models;

internal class TypeParameterModel
{
    public string VariableName { get; set; }
    public string TypeName { get; set; }
    public IReadOnlyList<BoundDefinitionModel> BoundModels { get; set; } = new List<BoundDefinitionModel>();
}