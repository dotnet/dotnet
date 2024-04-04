// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace Microsoft.CodeAnalysis.Razor.Logging;

// Our version of ILoggerFactory, so that we're not MEF importing general use types
internal interface ILoggerFactory
{
    void AddLoggerProvider(ILoggerProvider provider);
    ILogger GetOrCreateLogger(string categoryName);
}
