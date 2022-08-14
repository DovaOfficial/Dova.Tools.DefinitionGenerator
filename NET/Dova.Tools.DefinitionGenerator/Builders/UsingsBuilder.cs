using Dova.Common;
using Dova.Common.InterfaceFactory;
using Dova.Core;
using Dova.Tools.DefinitionGenerator.Models;

namespace Dova.Tools.DefinitionGenerator.Builders;

internal class UsingsBuilder : AbstractBuilder
{
    public override IEnumerable<string> Build(FileInfo outputFile, ClassDefinitionModel model, int tabs = 0)
    {
        yield return AppendLine("");
        yield return AppendLine($"using { typeof(DovaVM).Namespace };");
        yield return AppendLine($"using { typeof(JavaObject).Namespace };");
        yield return AppendLine($"using { typeof(DovaInterfaceFactory).Namespace };");
        yield return AppendLine("");
        yield return AppendLine("using CSharpSystem = System;");
    }
}