// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if NET
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using Microsoft.CodeAnalysis.Razor.Formatting;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Formatting;

[Obsolete($"{nameof(TestRazorFormattingService)} is only available on .NET Framework", error: true)]
internal static class TestRazorFormattingService
{
#pragma warning disable IDE0060 // Remove unused parameter

    public static Task<IRazorFormattingService> CreateWithFullSupportAsync(
        ILoggerFactory loggerFactory,
        RazorCodeDocument? codeDocument = null,
        IDocumentSnapshot? documentSnapshot = null,
        RazorLSPOptions? razorLSPOptions = null)
    {
        return Task.FromResult<IRazorFormattingService>(null!);
    }
}
#endif
