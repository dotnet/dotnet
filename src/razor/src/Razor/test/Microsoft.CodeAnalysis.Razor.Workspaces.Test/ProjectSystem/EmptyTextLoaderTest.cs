// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Test.Common;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

public class EmptyTextLoaderTest(ITestOutputHelper testOutput) : ToolingTestBase(testOutput)
{
    [Fact, WorkItem("https://github.com/dotnet/aspnetcore/issues/7997")]
    public async Task LoadAsync_SpecifiesEncoding()
    {
        // Act
        var textAndVersion = await EmptyTextLoader.Instance.LoadTextAndVersionAsync(options: default, DisposalToken);

        // Assert
        Assert.True(textAndVersion.Text.CanBeEmbedded);
        Assert.Same(Encoding.UTF8, textAndVersion.Text.Encoding);
    }
}
