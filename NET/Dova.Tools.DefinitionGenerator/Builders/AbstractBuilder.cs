using System.Text;
using Dova.Tools.DefinitionGenerator.Models;

namespace Dova.Tools.DefinitionGenerator.Builders;

internal abstract class AbstractBuilder : IBuilder
{
    public abstract IEnumerable<string> Build(FileInfo outputFile, ClassDefinitionModel model, int tabs = 0);

    public string AppendLine(string line, int tabs = 0)
    {
        var sb = new StringBuilder();

        for (var i = 0; i < tabs; ++i)
        {
            sb.Append("\t");
        }

        sb.Append(line);

        var newLine = sb.ToString();

        return newLine;
    }
}