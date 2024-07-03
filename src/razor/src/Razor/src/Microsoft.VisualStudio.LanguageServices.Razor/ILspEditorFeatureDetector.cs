// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace Microsoft.VisualStudio.Razor;

internal interface ILspEditorFeatureDetector
{
    /// <summary>
    /// Determines whether the LSP editor is enabled. This returns <see langword="true"/>
    /// if the legacy editor has <i>not</i> been enabled via the feature flag or tools/options.
    /// </summary>
    bool IsLspEditorEnabled();

    /// <summary>
    /// Determines whether the LSP editor is supported by the given document.
    /// </summary>
    bool IsLspEditorSupported(string documentFilePath);

    /// <summary>
    /// A remote client is a LiveShare guest or a Codespaces instance
    /// </summary>
    bool IsRemoteClient();

    bool IsLiveShareHost();
}
