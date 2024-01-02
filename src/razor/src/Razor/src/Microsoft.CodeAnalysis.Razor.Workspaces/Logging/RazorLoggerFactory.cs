// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.ComponentModel.Composition;
using Microsoft.Extensions.Logging;

namespace Microsoft.CodeAnalysis.Razor.Logging;

[Export(typeof(IRazorLoggerFactory)), System.Composition.Shared]
internal class RazorLoggerFactory : IRazorLoggerFactory
{
    private readonly ILoggerFactory _loggerFactory;

    [ImportingConstructor]
    public RazorLoggerFactory([ImportMany(typeof(IRazorLoggerProvider))] IEnumerable<IRazorLoggerProvider> razorLoggerProviders)
    {
        _loggerFactory = LoggerFactory.Create(b =>
        {
            // We let everything through, and expect individual loggers to control their own levels
            b.AddFilter(level => true);

            foreach (var provider in razorLoggerProviders)
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
