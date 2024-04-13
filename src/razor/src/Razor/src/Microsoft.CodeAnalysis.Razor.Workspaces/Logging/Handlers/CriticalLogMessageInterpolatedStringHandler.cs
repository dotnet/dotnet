// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Runtime.CompilerServices;

namespace Microsoft.CodeAnalysis.Razor.Logging;

[InterpolatedStringHandler]
internal ref struct CriticalLogMessageInterpolatedStringHandler
{
    private readonly LogMessageInterpolatedStringHandler _handler;

    public CriticalLogMessageInterpolatedStringHandler(int literalLength, int _, ILogger logger, out bool isEnabled)
    {
        _handler = new LogMessageInterpolatedStringHandler(literalLength, _, logger, LogLevel.Critical, out isEnabled);
    }

    public bool IsEnabled => _handler.IsEnabled;

    public void AppendLiteral(string s)
        => _handler.AppendLiteral(s);

    public void AppendFormatted<T>(T t)
        => _handler.AppendFormatted(t);

    public void AppendFormatted<T>(T t, string format)
        => _handler.AppendFormatted(t, format);

    public override string ToString()
        => _handler.ToString();
}
