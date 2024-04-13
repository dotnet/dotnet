// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

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
