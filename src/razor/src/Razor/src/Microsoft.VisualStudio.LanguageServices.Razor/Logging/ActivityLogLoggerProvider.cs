// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.ComponentModel.Composition;
using Microsoft.CodeAnalysis.Razor.Logging;

namespace Microsoft.VisualStudio.Razor.Logging;

/// <summary>
/// An <see cref="ILoggerProvider"/> that logs any warnings or errors to the Visual Studio Activity Log.
/// </summary>
[ExportLoggerProvider(minimumLogLevel: LogLevel.Warning)]
[method: ImportingConstructor]
internal sealed partial class ActivityLogLoggerProvider(RazorActivityLog activityLog) : ILoggerProvider
{
    private readonly RazorActivityLog _activityLog = activityLog;

    public ILogger CreateLogger(string categoryName)
        => new Logger(_activityLog, categoryName);
}
