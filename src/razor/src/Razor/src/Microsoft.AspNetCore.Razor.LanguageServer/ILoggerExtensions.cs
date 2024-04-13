// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis.Razor.Logging;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal static class ILoggerExtensions
{
    public static bool TestOnlyLoggingEnabled = false;

    [Conditional("DEBUG")]
    public static void LogTestOnly(this ILogger logger, ref TestLogMessageInterpolatedStringHandler handler)
    {
        if (TestOnlyLoggingEnabled)
        {
            logger.Log(LogLevel.Debug, handler.ToString(), exception: null);
        }
    }
}

[InterpolatedStringHandler]
internal ref struct TestLogMessageInterpolatedStringHandler
{
    private PooledObject<StringBuilder> _builder;

    public TestLogMessageInterpolatedStringHandler(int literalLength, int _, out bool isEnabled)
    {
        isEnabled = ILoggerExtensions.TestOnlyLoggingEnabled;
        if (isEnabled)
        {
            _builder = StringBuilderPool.GetPooledObject();
            _builder.Object.EnsureCapacity(literalLength);
        }
    }

    public void AppendLiteral(string s)
    {
        _builder.Object.Append(s);
    }

    public void AppendFormatted<T>(T t)
    {
        _builder.Object.Append(t?.ToString() ?? "[null]");
    }

    public void AppendFormatted<T>(T t, string format)
    {
        _builder.Object.AppendFormat(format, t);
    }

    public override string ToString()
    {
        var result = _builder.Object.ToString();
        _builder.Dispose();
        return result;
    }
}
