// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Composition;
using Microsoft.CodeAnalysis.Razor.Logging;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

[Shared]
[Export(typeof(ILoggerFactory))]
[method: ImportingConstructor]
internal sealed class LoggerFactory(ILoggerProvider provider)
    : AbstractLoggerFactory([provider])
{
}
