// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.Razor;
using Microsoft.VisualStudio.LanguageServices.Razor;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.Test.Common.VisualStudio;

public abstract class VisualStudioTestBase(ITestOutputHelper testOutput) : ToolingTestBase(testOutput)
{
    private protected override ProjectSnapshotManagerDispatcher CreateDispatcher()
    {
        var dispatcher = new VisualStudioProjectSnapshotManagerDispatcher(ErrorReporter);
        AddDisposable(dispatcher);

        return dispatcher;
    }
}
