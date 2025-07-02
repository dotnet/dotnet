// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading;
using Microsoft.CodeAnalysis.Razor.Logging;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Hosting.Logging;

internal class LogLevelProvider(LogLevel logLevel)
{
    public LogLevel Current { get; set; } = logLevel;
}
