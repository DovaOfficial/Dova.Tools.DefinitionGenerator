using System.Text;
using Dova.Common;
using Dova.Tools.DefinitionGenerator.Models;

namespace Dova.Tools.DefinitionGenerator.Common;

internal static class DefinitionCleaner
{
    private static IDictionary<string, string> Replacements { get; } = new Dictionary<string, string>
    {
        { "boolean", "bool" },
        { "$", "." },
    };

    private static IEnumerable<string> CSharpKeywords { get; } = new List<string>
    {
        "abstract", "as", "base", "bool", "break", "byte", "case", "catch", "char", "checked", "class", "const",
        "continue", "decimal", "default", "delegate", "do", "double", "else", "enum", "event", "explicit", "extern",
        "false", "finally", "fixed", "float", "for", "foreach", "goto", "if", "implicit", "in", "int", "interface",
        "internal", "is", "lock", "long", "namespace", "new", "null", "object", "operator", "out", "override",
        "params", "private", "protected", "public", "readonly", "ref", "return", "sbyte", "sealed", "short", "sizeof",
        "stackalloc", "static", "string", "struct", "switch", "this", "throw", "true", "try", "typeof", "uint",
        "ulong", "unchecked", "unsafe", "ushort", "using", "virtual", "void", "volatile", "while"
    };

    public static string[] GetParts(string str) => 
        str.Replace(";", "").Split(".");

    public static string CleanKeywords(string str)
    {
        var ret = str;

        var parts = GetParts(ret);

        for (var index = 0; index < parts.Length; index++)
        {
            foreach (var keyword in CSharpKeywords)
            {
                if (parts[index].Equals(keyword))
                {
                    parts[index] = $"@{keyword}";
                }
            }
        }

        ret = string.Join(".", parts);

        return ret;
    }

    public static string CleanJavaSpecifics(string str) =>
        Replacements.Aggregate(str, (current, replacement) => current.Replace(replacement.Key, replacement.Value));

    public static string AppendNamespacePrefix(string str) =>
        string.Join(".", "Dova.JDK", str);

    public static string GetLastInnerPart(string str)
    {
        var ret = str;

        if (ret.Contains("$"))
        {
            var parts = ret.Split("$");
            ret = parts[^1];
        }

        return ret;
    }

    public static string CleanClass(string str)
    {
        var ret = str;
        
        ret = CleanJavaSpecifics(ret);
        ret = RemoveGenerics(ret);

        return ret switch
        {
            var s when !s.Contains(".") && !s.Contains("[]") => CleanGenericType(s), // int or MyClass
            var s when s.Contains(".") && !s.Contains("[]") => CleanDotEnds(CleanNumbers(CleanKeywords(AppendNamespacePrefix(s)))), // java.lang.Object or com.my.package.MyClass
            var s when s.EndsWith("[]") => $"JavaArray<{CleanClass(s[..^2])}>", // byte[] or java.lang.Object[]
            _ => ret,
        };
    }

    private static string CleanDotEnds(string str) => 
        str.EndsWith(".") ? str[..^1] : str;

    public static string CleanGenericType(string str)
    {
        if (CSharpKeywords.Any(keyword => str.Equals(keyword))
            || str.Equals(nameof(IJavaObject))
            || str.Equals(nameof(JavaObject)))
        {
            return str;
        }
        
        return AppendNamespacePrefix(Constants.JavaObjectFullName);
    }

    public static string RemoveGenerics(string str)
    {
        var ret = str.Replace(" ", "");
        var sb = new StringBuilder();
        var depth = 0;

        foreach (var ch in ret)
        {
            switch (ch)
            {
                case '<':
                    depth++;
                    break;
                case '>':
                    depth--;
                    break;
                default:
                    if (depth == 0)
                    {
                        sb.Append(ch);
                    }
                    break;
            }
        }

        var sbStr = sb.ToString();

        return sbStr;
    }

    public static bool IsObject(string str) => 
        str.Contains(".") || str.StartsWith("JavaArray");
    
    public static string GetReturnValue(string str) => 
        IsObject(str) 
            ? $"value.{nameof(JavaObject.CurrentRefPtr)}" 
            : "value";
    
    public static string GetReturnTypeMethodPostfix(string str) => 
        IsObject(str) 
            ? "Object" 
            : str.ToFirstUppercase().Replace("Bool", "Boolean");

    public static string GetPropertyName(string str)
    {
        var ret = str.Replace("$", "");

        ret = CleanKeywords(ret);
        ret += "_Property";

        if (ret.StartsWith("set_") 
            || ret.StartsWith("get_"))
        {
            ret = ret.Replace("_", "__");
        }

        return ret;
    }

    public static string GetCombinedParameters(IEnumerable<ParameterDefinitionModel> models) => 
        string.Join(", ", models.Select(p => $"{CleanClass(p.Type)} {p.Name}"));
    
    public static string GetCombinedParameterNames(IEnumerable<ParameterDefinitionModel> models) => 
        string.Join(", ", models.Select(p => p.Name));

    public static string GetMethodName(string str)
    {
        var ret = str.Replace("$", "_"); // Specific case for methods - we cannot create a '.' in front of a method name

        ret = CleanKeywords(ret);
        
        return ret;
    }

    public static string CleanNumbers(string str)
    {
        var ret = str;

        for (var i = 0; i <= 9; ++i)
        {
            ret = ret.Replace($".{i}", ".");
        }

        return ret;
    }

    public static IEnumerable<ClassElementDefinitionModel> GetMethodsToGenerate(string className, IEnumerable<ClassElementDefinitionModel> models)
    {
        var groupedByName = models
            .Where(m => !m.Name.StartsWith("lambda$"))
            .GroupBy(m => m.Name);
        
        foreach (var nameGroup in groupedByName) // i.e.: "resolveConstantDesc"
        {
            var groupedByParameters = nameGroup.GroupBy(x => x.ParameterModels.Count);
        
            foreach (var parametersGroup in groupedByParameters) // i.e.: 1 param (or N params)
            {
                var combinedParametersGroups = parametersGroup.GroupBy(x =>
                    string.Join(", ", x.ParameterModels.Select(m => CleanClass(m.Type))));
        
                foreach (var combinedParametersGroup in combinedParametersGroups) // i.e.: "java.lang.@invoke.MethodHandles_Lookup" (or N combined params)
                {
                    for (var index = 0; index < combinedParametersGroup.Count(); ++index)
                    {
                        var defModel = combinedParametersGroup.ElementAt(index);
                        
                        var newDefModel = new ClassElementDefinitionModel
                        {
                            Modifiers = defModel.Modifiers,
                            Name = defModel.Name,
                            Signature = defModel.Signature,
                            HasParent = defModel.HasParent,
                            IsStatic = defModel.IsStatic,
                            ParameterModels = defModel.ParameterModels,
                            ReturnType = defModel.ReturnType,
                            TypeParameterModels = defModel.TypeParameterModels
                        };

                        if (combinedParametersGroup.Count() > 1
                            || newDefModel.Name.Equals(className))
                        {
                            newDefModel.Name += $"_{index}";
                        }
                        
                        yield return newDefModel;
                    }
                }
            }
        }
    }
    
    public static string GetReturnString(ClassDefinitionModel classDefModel, ClassElementDefinitionModel classElDefModel, string str) =>
        IsObject(str) ? $"return DovaInterfaceFactory.Get<{str}>(ret);" : $"return ret;";
}