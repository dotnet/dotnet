// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.Razor.ProjectSystem.Legacy;
using Microsoft.CodeAnalysis.Razor.Settings;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;

namespace Microsoft.VisualStudio.LegacyEditor.Razor;

internal interface IVisualStudioDocumentTracker
{
    RazorConfiguration? Configuration { get; }
    ClientSpaceSettings EditorSettings { get; }
    ImmutableArray<TagHelperDescriptor> TagHelpers { get; }
    bool IsSupportedProject { get; }
    string FilePath { get; }
    string ProjectPath { get; }
    ILegacyProjectSnapshot? ProjectSnapshot { get; }
    ITextBuffer TextBuffer { get; }
    IReadOnlyList<ITextView> TextViews { get; }

    ITextView? GetFocusedTextView();

    event EventHandler<ContextChangeEventArgs> ContextChanged;
}
