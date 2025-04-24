// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Composition;
using Microsoft.CodeAnalysis.Razor.Logging;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

[Shared]
[Export(typeof(ILoggerProvider))]
[method: ImportingConstructor]
internal class LoggerProvider(RazorClientServerManagerProvider razorClientServerManagerProvider) : ILoggerProvider
{
    private readonly RazorClientServerManagerProvider _razorClientServerManagerProvider = razorClientServerManagerProvider;

    public ILogger CreateLogger(string categoryName)
    {
        return new LspLogger(categoryName, _razorClientServerManagerProvider);
    }
}
