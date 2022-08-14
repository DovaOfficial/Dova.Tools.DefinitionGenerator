namespace Dova.Tools.DefinitionGenerator.Common;

public static class StringExtensions
{
    public static string ToFirstUppercase(this string str) => 
        char.ToUpper(str[0]) + str[1..];
}