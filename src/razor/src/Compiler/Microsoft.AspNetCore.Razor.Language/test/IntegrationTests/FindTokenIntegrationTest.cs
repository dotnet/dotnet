// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Razor.Language.IntegrationTests;
using Microsoft.AspNetCore.Razor.Language.Syntax;
using Roslyn.Test.Utilities;
using Xunit;

namespace Microsoft.AspNetCore.Razor.Language.Test;

public class FindTokenIntegrationTest : IntegrationTestBase
{
    [Fact, WorkItem("https://github.com/dotnet/razor/issues/9177")]
    public void EmptyDirective()
    {
        var projectEngine = CreateProjectEngine();
        var projectItem = CreateProjectItemFromFile();

        var codeDocument = projectEngine.Process(projectItem);

        var root = codeDocument.GetSyntaxTree().Root;
        var token = root.FindToken(27);
        AssertEx.Equal("Identifier;[<Missing>];", SyntaxSerializer.Serialize(token).Trim());
    }
}
