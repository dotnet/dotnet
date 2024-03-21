// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using Microsoft.Extensions.Logging;

namespace Microsoft.CodeAnalysis.Razor.Logging;

internal abstract class AbstractRazorLoggerFactory : IRazorLoggerFactory
{
    private readonly ILoggerFactory _loggerFactory;

    protected AbstractRazorLoggerFactory(ImmutableArray<IRazorLoggerProvider> providers)
    {
        _loggerFactory = LoggerFactory.Create(b =>
        {
            // We let everything through, and expect individual loggers to control their own levels
            b.AddFilter(level => true);

            foreach (var provider in providers)
            {
                b.AddProvider(provider);
            }
        });
    }

    public ILogger CreateLogger(string categoryName)
    {
        return _loggerFactory.CreateLogger(categoryName);
    }

    public void AddLoggerProvider(IRazorLoggerProvider provider)
    {
        _loggerFactory.AddProvider(provider);
    }

    public void Dispose()
    {
        _loggerFactory.Dispose();
    }
}
