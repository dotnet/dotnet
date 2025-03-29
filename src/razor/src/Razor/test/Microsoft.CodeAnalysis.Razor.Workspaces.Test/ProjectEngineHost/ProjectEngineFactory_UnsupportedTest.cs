// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Test.Common;

using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Microsoft.CodeAnalysis.Razor.ProjectEngineHost.Test;

public class ProjectEngineFactory_UnsupportedTest(ITestOutputHelper testOutput) : ToolingTestBase(testOutput)
{
    [Fact]
    public void Create_IgnoresConfigureParameter()
    {
        // Arrange
        var factory = new ProjectEngineFactory_Unsupported();

        // Act & Assert
        factory.Create(UnsupportedRazorConfiguration.Instance, RazorProjectFileSystem.Empty, (builder) => throw new XunitException("There should not be an opportunity to configure the project engine in the unsupported scenario."));
    }

    // This is more of an integration test to validate that all the pieces work together
    [Fact]
    public void Create_ProcessDesignTime_AlwaysGeneratesEmptyGeneratedCSharp()
    {
        // Arrange
        var factory = new ProjectEngineFactory_Unsupported();
        var engine = factory.Create(UnsupportedRazorConfiguration.Instance, RazorProjectFileSystem.Empty, (_) => { });
        var sourceDocument = TestRazorSourceDocument.Create("<strong>Hello World!</strong>", RazorSourceDocumentProperties.Default);

        // Act
        var codeDocument = engine.ProcessDesignTime(sourceDocument, "test", importSources: default, Array.Empty<TagHelperDescriptor>());

        // Assert
        Assert.Equal(UnsupportedCSharpLoweringPhase.UnsupportedDisclaimer, codeDocument.GetCSharpDocument().Text.ToString());
    }
}
