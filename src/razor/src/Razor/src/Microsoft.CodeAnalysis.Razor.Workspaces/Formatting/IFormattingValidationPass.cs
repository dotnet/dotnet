// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.CodeAnalysis.Razor.Formatting;

internal interface IFormattingValidationPass
{
    Task<bool> IsValidAsync(FormattingContext formattingContext, ImmutableArray<TextChange> changes, CancellationToken cancellationToken);
}
