// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Immutable;
using Microsoft.VisualStudio.Text.Editor;

namespace Microsoft.AspNetCore.Razor.ExternalAccess.LegacyEditor;

internal interface IRazorDocumentTracker
{
    ImmutableArray<ITextView> TextViews { get; }

    event EventHandler<ContextChangeEventArgs> ContextChanged;
}
