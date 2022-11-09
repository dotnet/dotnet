// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.TestPlatform.CommandLine.Internal;

internal interface IProgressIndicator
{
    /// <summary>
    /// Marks the start of the progress indicator
    /// </summary>
    void Start();

    /// <summary>
    /// Pause the progress indicator
    /// </summary>
    void Pause();

    /// <summary>
    /// Stop the progress indicator
    /// </summary>
    void Stop();
}
