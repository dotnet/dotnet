// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Text;

namespace Microsoft.VisualStudio.Razor.Debugging;

internal interface IRazorProximityExpressionResolver
{
    Task<IReadOnlyList<string>?> TryResolveProximityExpressionsAsync(ITextBuffer textBuffer, int lineIndex, int characterIndex, CancellationToken cancellationToken);
}
