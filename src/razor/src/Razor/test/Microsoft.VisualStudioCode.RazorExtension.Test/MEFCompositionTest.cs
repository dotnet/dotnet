// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Linq;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.Mef;
using Roslyn.Test.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudioCode.RazorExtension.Test;

public class MEFCompositionTest(ITestOutputHelper testOutputHelper) : ToolingTestBase(testOutputHelper)
{
    [Fact]
    public void Composes()
    {
        var testComposition = TestComposition.RoslynFeatures
            .AddAssemblies(typeof(RazorVSCodeEndpoint).Assembly);

        var errors = testComposition.GetCompositionErrors().Order().ToArray();

        // There are known failures that are satisfied by Microsoft.CodeAnalysis.LanguageServer, which we don't reference
        Assert.Collection(errors,
            e => AssertEx.Equal("""
                Microsoft.CodeAnalysis.ExternalAccess.Pythia.PythiaSignatureHelpProvider.ctor(implementation): expected exactly 1 export matching constraints:
                    Contract name: Microsoft.CodeAnalysis.ExternalAccess.Pythia.Api.IPythiaSignatureHelpProviderImplementation
                    TypeIdentityName: Microsoft.CodeAnalysis.ExternalAccess.Pythia.Api.IPythiaSignatureHelpProviderImplementation
                but found 0.
                """, e),
            e => AssertEx.Equal("""
                Microsoft.VisualStudioCode.RazorExtension.Services.WorkspaceService.ctor(loggerFactory): expected exactly 1 export matching constraints:
                    Contract name: Microsoft.Extensions.Logging.ILoggerFactory
                    TypeIdentityName: Microsoft.Extensions.Logging.ILoggerFactory
                but found 0.
                """, e));
        ;
    }
}
