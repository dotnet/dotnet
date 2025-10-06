// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

namespace Microsoft.VisualStudio.LegacyEditor.Razor;

internal interface IImportDocumentManager
{
    void OnSubscribed(IVisualStudioDocumentTracker documentTracker);
    void OnUnsubscribed(IVisualStudioDocumentTracker documentTracker);

    event EventHandler<ImportChangedEventArgs> Changed;
}
