// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis.Razor.Logging;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal sealed class LoggerFactory(ImmutableArray<Lazy<ILoggerProvider, LoggerProviderMetadata>> providers)
    : AbstractLoggerFactory(providers)
{
}
