// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using Microsoft.CodeAnalysis.Razor.Logging;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Hosting.Logging;

internal class LogLevelProvider(LogLevel logLevel)
{
    public LogLevel Current { get; set; } = logLevel;
}
