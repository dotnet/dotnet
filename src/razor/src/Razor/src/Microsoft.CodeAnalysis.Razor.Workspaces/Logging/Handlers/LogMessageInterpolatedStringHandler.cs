// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Roslyn.LanguageServer.Protocol;

namespace Microsoft.CodeAnalysis.Razor.Logging;

[InterpolatedStringHandler]
internal ref struct LogMessageInterpolatedStringHandler
{
    private PooledObject<StringBuilder> _builder;
    private readonly bool _isEnabled;

    public LogMessageInterpolatedStringHandler(int literalLength, int _, ILogger logger, LogLevel logLevel, out bool isEnabled)
    {
        _isEnabled = isEnabled = logger.IsEnabled(logLevel);
        if (isEnabled)
        {
            _builder = StringBuilderPool.GetPooledObject();
            _builder.Object.EnsureCapacity(literalLength);
        }
    }

    public bool IsEnabled => _isEnabled;

    public void AppendLiteral(string s)
    {
        _builder.Object.Append(s);
    }

    public void AppendFormatted<T>(T t)
    {
        _builder.Object.Append(GetMessage(t));
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

    private static string GetMessage(object? value)
        => value switch
        {
            VisualStudio.LanguageServer.Protocol.Range range => range.ToDisplayString(),
            VisualStudio.LanguageServer.Protocol.Position position => position.ToDisplayString(),
            VisualStudio.LanguageServer.Protocol.ISumType sumType => GetMessage(sumType.Value),

            Roslyn.LanguageServer.Protocol.Range range => range.ToDisplayString(),
            Roslyn.LanguageServer.Protocol.Position position => position.ToDisplayString(),
            Roslyn.LanguageServer.Protocol.ISumType sumType => GetMessage(sumType.Value),

            null => "[null]",
            _ => value.ToString() ?? "[null]"
        };
}
