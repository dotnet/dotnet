// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Razor.Logging;

internal sealed class LoggerProviderMetadata
{
    public static LoggerProviderMetadata Empty { get; } = new();

    public LogLevel? MinimumLogLevel { get; }

    private LoggerProviderMetadata()
    {
    }

    public LoggerProviderMetadata(IDictionary<string, object> data)
        : this()
    {
        MinimumLogLevel = data.TryGetValue(nameof(MinimumLogLevel), out var minimumLogLevel)
            ? (LogLevel?)minimumLogLevel
            : null;
    }
}
