// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.VisualStudio.Text;

namespace Microsoft.VisualStudio.LanguageServer.ContainedLanguage;

internal abstract class VirtualDocumentSnapshot
{
    public abstract Uri Uri { get; }

    public abstract ITextSnapshot Snapshot { get; }

    public abstract long? HostDocumentSyncVersion { get; }
}
