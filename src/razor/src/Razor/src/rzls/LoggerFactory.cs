// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using Microsoft.CodeAnalysis.Razor.Logging;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal sealed class LoggerFactory(ImmutableArray<IRazorLoggerProvider> providers)
    : AbstractRazorLoggerFactory(providers)
{
}
