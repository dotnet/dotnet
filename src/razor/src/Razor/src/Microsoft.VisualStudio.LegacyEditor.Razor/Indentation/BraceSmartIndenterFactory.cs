// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Razor.Extensions;
using Microsoft.VisualStudio.Text.Operations;
using Microsoft.VisualStudio.Threading;

namespace Microsoft.VisualStudio.LegacyEditor.Razor.Indentation;

[Export(typeof(IBraceSmartIndenterFactory))]
[method: ImportingConstructor]
internal sealed class BraceSmartIndenterFactory(
    IEditorOperationsFactoryService editorOperationsFactory,
    JoinableTaskContext joinableTaskContext) : IBraceSmartIndenterFactory
{
    public BraceSmartIndenter Create(IVisualStudioDocumentTracker documentTracker)
    {
        joinableTaskContext.AssertUIThread();

        return new BraceSmartIndenter(documentTracker, editorOperationsFactory, joinableTaskContext);
    }
}
