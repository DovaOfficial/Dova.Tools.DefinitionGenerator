namespace Dova.Tools.DefinitionGenerator.Models;

internal class ClassDefinitionModel
{
    public string ModuleName { get; set; }
    public ClassDetailsDefinitionModel ClassDetailsModel { get; set; }
    public ClassElementDefinitionModel BaseClassModel { get; set; }
    public IReadOnlyList<ClassElementDefinitionModel> InterfaceModels { get; set; } = new List<ClassElementDefinitionModel>();
    public IReadOnlyList<ClassElementDefinitionModel> ConstructorModels { get; set; } = new List<ClassElementDefinitionModel>();
    public IReadOnlyList<ClassElementDefinitionModel> FieldModels { get; set; } = new List<ClassElementDefinitionModel>();
    public IReadOnlyList<ClassElementDefinitionModel> MethodModels { get; set; } = new List<ClassElementDefinitionModel>();
    public IReadOnlyList<ClassDefinitionModel> InnerClassModels { get; set; } = new List<ClassDefinitionModel>();
}