// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;

namespace Microsoft.VisualStudio.LegacyEditor.Razor;

internal interface IImportDocumentManager
{
    void OnSubscribed(IVisualStudioDocumentTracker documentTracker);
    void OnUnsubscribed(IVisualStudioDocumentTracker documentTracker);

    event EventHandler<ImportChangedEventArgs> Changed;
}
