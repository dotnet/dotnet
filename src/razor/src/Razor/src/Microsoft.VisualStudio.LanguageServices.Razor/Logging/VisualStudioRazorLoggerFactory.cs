// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.Composition;
using Microsoft.CodeAnalysis.Razor.Logging;

namespace Microsoft.VisualStudio.LanguageServices.Razor.Logging;

[Export(typeof(IRazorLoggerFactory))]
[method: ImportingConstructor]
internal sealed class VisualStudioRazorLoggerFactory([ImportMany] IEnumerable<IRazorLoggerProvider> providers)
    : AbstractRazorLoggerFactory(providers.ToImmutableArray())
{
}
