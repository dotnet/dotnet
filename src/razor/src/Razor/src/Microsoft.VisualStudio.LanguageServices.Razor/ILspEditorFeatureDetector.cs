// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace Microsoft.VisualStudio.Razor;

internal interface ILspEditorFeatureDetector
{
    bool IsLspEditorEnabled();

    /// <summary>
    /// A remote client is a LiveShare guest or a Codespaces instance
    /// </summary>
    bool IsRemoteClient();

    bool IsLiveShareHost();
}
