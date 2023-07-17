// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using Microsoft.VisualStudio.Text.Editor;

namespace Microsoft.AspNetCore.Razor.ExternalAccess.LegacyEditor;

internal interface IRazorDocumentTracker
{
    ImmutableArray<ITextView> TextViews { get; }

    event EventHandler<ContextChangeEventArgs> ContextChanged;
}
