// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.Composition;
using Microsoft.CodeAnalysis.Razor.Logging;

namespace Microsoft.VisualStudio.Razor.Logging;

[Export(typeof(ILoggerFactory))]
[method: ImportingConstructor]
internal sealed class VisualStudioLoggerFactory([ImportMany] IEnumerable<ILoggerProvider> providers)
    : AbstractLoggerFactory(providers.ToImmutableArray())
{
}
