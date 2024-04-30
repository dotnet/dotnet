// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.CodeAnalysis.Razor.Logging;

namespace Microsoft.AspNetCore.Razor.Logging;

internal sealed partial class EmptyLoggingFactory : ILoggerFactory
{
    public static readonly EmptyLoggingFactory Instance = new();

    private EmptyLoggingFactory()
    {
    }

    public void AddLoggerProvider(ILoggerProvider provider)
        => throw new NotImplementedException();

    public ILogger GetOrCreateLogger(string categoryName)
        => EmptyLogger.Instance;
}
