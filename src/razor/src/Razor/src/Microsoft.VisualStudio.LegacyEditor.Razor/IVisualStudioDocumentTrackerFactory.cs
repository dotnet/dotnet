// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.VisualStudio.Text;

namespace Microsoft.VisualStudio.LegacyEditor.Razor;

internal interface IVisualStudioDocumentTrackerFactory
{
    IVisualStudioDocumentTracker? Create(ITextBuffer textBuffer);
}
