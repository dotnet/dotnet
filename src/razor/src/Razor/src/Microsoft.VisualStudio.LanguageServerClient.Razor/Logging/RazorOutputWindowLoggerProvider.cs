// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Editor.Razor.Logging;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor.Logging;
internal class RazorOutputWindowLoggerProvider : ILoggerProvider
{
    private readonly IOutputWindowLogger _outputWindowLogger;

    public RazorOutputWindowLoggerProvider(IOutputWindowLogger outputWindowLogger)
    {
        _outputWindowLogger = outputWindowLogger ?? throw new ArgumentNullException(nameof(outputWindowLogger));
    }

    public ILogger CreateLogger(string _)
    {
        return _outputWindowLogger;
    }

    public void Dispose()
    {
    }
}
