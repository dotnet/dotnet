// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Composition;
using Microsoft.CodeAnalysis.Razor.Logging;

namespace Microsoft.CodeAnalysis.Remote.Razor.Logging;

[Shared]
[Export(typeof(ILoggerFactory))]
[Export(typeof(RemoteLoggerFactory))]
internal sealed class RemoteLoggerFactory : ILoggerFactory
{
    private ILoggerFactory _targetLoggerFactory = EmptyLoggerFactory.Instance;

    internal void SetTargetLoggerFactory(ILoggerFactory loggerFactory)
    {
        // We only set the target logger factory if the current factory is empty.
        if (_targetLoggerFactory is EmptyLoggerFactory)
        {
            _targetLoggerFactory = loggerFactory;
        }
    }

    public void AddLoggerProvider(ILoggerProvider provider)
    {
        _targetLoggerFactory.AddLoggerProvider(provider);
    }

    public ILogger GetOrCreateLogger(string categoryName)
    {
        return _targetLoggerFactory.GetOrCreateLogger(categoryName);
    }

    public void Dispose()
    {
    }
}
