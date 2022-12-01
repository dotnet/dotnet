// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.Razor;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor;

internal class TestRazorLogger : RazorLogger
{
    public static readonly RazorLogger Instance = new TestRazorLogger();

    private TestRazorLogger()
    {
    }

    public override void LogError(string message)
    {
    }

    public override void LogVerbose(string message)
    {
    }

    public override void LogWarning(string message)
    {
    }
}
