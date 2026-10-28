// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;

using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine;

internal interface IArtifactProcessingManager
{
    /// <summary>
    /// Collects the test session artifacts for post processing.
    /// </summary>
    /// <returns>
    /// A warning to surface to the user when the artifacts directory could not be verified (so nothing was
    /// collected), or <see langword="null"/> when there is nothing to report.
    /// </returns>
    string? CollectArtifacts(TestRunCompleteEventArgs testRunCompleteEventArgs, string runSettingsXml);
    Task PostProcessArtifactsAsync();
}
