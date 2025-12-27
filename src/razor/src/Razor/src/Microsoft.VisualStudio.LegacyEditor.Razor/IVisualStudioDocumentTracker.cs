// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
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
    TagHelperCollection TagHelpers { get; }
    bool IsSupportedProject { get; }
    string FilePath { get; }
    string ProjectPath { get; }
    ILegacyProjectSnapshot? ProjectSnapshot { get; }
    ITextBuffer TextBuffer { get; }
    IReadOnlyList<ITextView> TextViews { get; }

    ITextView? GetFocusedTextView();

    event EventHandler<ContextChangeEventArgs> ContextChanged;
}
