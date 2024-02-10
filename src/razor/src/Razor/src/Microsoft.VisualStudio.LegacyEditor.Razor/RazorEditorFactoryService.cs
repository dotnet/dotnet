// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.LegacyEditor.Razor.Indentation;
using Microsoft.VisualStudio.LegacyEditor.Razor.Parsing;
using Microsoft.VisualStudio.Text;

namespace Microsoft.VisualStudio.LegacyEditor.Razor;

[Export(typeof(IRazorEditorFactoryService))]
[method: ImportingConstructor]
internal sealed class RazorEditorFactoryService(
    IVisualStudioDocumentTrackerFactory documentTrackerFactory,
    IVisualStudioRazorParserFactory parserFactory,
    IBraceSmartIndenterFactory braceSmartIndenterFactory) : IRazorEditorFactoryService
{
    private static readonly object s_razorTextBufferInitializationKey = new();

    private readonly IVisualStudioDocumentTrackerFactory _documentTrackerFactory = documentTrackerFactory;
    private readonly IVisualStudioRazorParserFactory _parserFactory = parserFactory;
    private readonly IBraceSmartIndenterFactory _braceSmartIndenterFactory = braceSmartIndenterFactory;

    public bool TryGetDocumentTracker(ITextBuffer textBuffer, [NotNullWhen(true)] out IVisualStudioDocumentTracker? documentTracker)
    {
        if (textBuffer is null)
        {
            throw new ArgumentNullException(nameof(textBuffer));
        }

        if (!textBuffer.IsLegacyCoreRazorBuffer())
        {
            documentTracker = null;
            return false;
        }

        var textBufferInitialized = TryInitializeTextBuffer(textBuffer);
        if (!textBufferInitialized)
        {
            documentTracker = null;
            return false;
        }

        if (!textBuffer.Properties.TryGetProperty(typeof(IVisualStudioDocumentTracker), out documentTracker) ||
            documentTracker is null)
        {
            Debug.Fail("Document tracker should have been stored on the text buffer during initialization.");
            return false;
        }

        return true;
    }

    public bool TryGetParser(ITextBuffer textBuffer, [NotNullWhen(true)] out IVisualStudioRazorParser? parser)
    {
        if (textBuffer is null)
        {
            throw new ArgumentNullException(nameof(textBuffer));
        }

        if (!textBuffer.IsLegacyCoreRazorBuffer())
        {
            parser = null;
            return false;
        }

        var textBufferInitialized = TryInitializeTextBuffer(textBuffer);
        if (!textBufferInitialized)
        {
            parser = null;
            return false;
        }

        if (!textBuffer.Properties.TryGetProperty(typeof(IVisualStudioRazorParser), out parser) ||
            parser is null)
        {
            Debug.Fail("Parser should have been stored on the text buffer during initialization.");
            return false;
        }

        return true;
    }

    public bool TryGetSmartIndenter(ITextBuffer textBuffer, [NotNullWhen(returnValue: true)] out BraceSmartIndenter? braceSmartIndenter)
    {
        if (textBuffer is null)
        {
            throw new ArgumentNullException(nameof(textBuffer));
        }

        if (!textBuffer.IsLegacyCoreRazorBuffer())
        {
            braceSmartIndenter = null;
            return false;
        }

        var textBufferInitialized = TryInitializeTextBuffer(textBuffer);
        if (!textBufferInitialized)
        {
            braceSmartIndenter = null;
            return false;
        }

        if (!textBuffer.Properties.TryGetProperty(typeof(BraceSmartIndenter), out braceSmartIndenter!))
        {
            Debug.Fail("Brace smart indenter should have been stored on the text buffer during initialization.");
            return false;
        }

        return true;
    }

    // Internal for testing
    internal bool TryInitializeTextBuffer(ITextBuffer textBuffer)
    {
        if (textBuffer.Properties.ContainsProperty(s_razorTextBufferInitializationKey))
        {
            // Buffer already initialized.
            return true;
        }

        var tracker = _documentTrackerFactory.Create(textBuffer);
        Assumes.NotNull(tracker);
        textBuffer.Properties[typeof(IVisualStudioDocumentTracker)] = tracker;

        var parser = _parserFactory.Create(tracker);
        textBuffer.Properties[typeof(IVisualStudioRazorParser)] = parser;

        var braceSmartIndenter = _braceSmartIndenterFactory.Create(tracker);
        textBuffer.Properties[typeof(BraceSmartIndenter)] = braceSmartIndenter;

        textBuffer.Properties.AddProperty(s_razorTextBufferInitializationKey, s_razorTextBufferInitializationKey);

        return true;
    }
}
