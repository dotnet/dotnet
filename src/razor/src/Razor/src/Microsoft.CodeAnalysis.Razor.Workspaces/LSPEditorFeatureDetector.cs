// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace Microsoft.CodeAnalysis.Razor.Workspaces;

internal abstract class LSPEditorFeatureDetector
{
    public abstract bool IsLSPEditorAvailable(string? documentMoniker, object? hierarchy);

    public abstract bool IsLSPEditorAvailable();

    /// <summary>
    /// A remote client is a LiveShare guest or a Codespaces instance
    /// </summary>
    public abstract bool IsRemoteClient();

    public abstract bool IsLiveShareHost();
}
