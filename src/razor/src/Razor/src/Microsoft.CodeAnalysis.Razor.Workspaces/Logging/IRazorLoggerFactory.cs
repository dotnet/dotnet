// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.Extensions.Logging;

namespace Microsoft.CodeAnalysis.Razor.Logging;

// Our version of ILoggerFactory, so that we're not MEF importing general use types
internal interface IRazorLoggerFactory : IDisposable
{
    void AddLoggerProvider(IRazorLoggerProvider provider);
    ILogger CreateLogger(string categoryName);
}
