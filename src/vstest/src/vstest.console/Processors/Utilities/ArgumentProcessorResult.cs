// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.TestPlatform.CommandLine.Processors;

/// <summary>
/// Return values from argument processors.
/// </summary>
public enum ArgumentProcessorResult
{
    /// <summary>
    /// Return value indicating no errors.
    /// </summary>
    Success = 0,

    /// <summary>
    /// Return value indicating an error occurred
    /// </summary>
    Fail = 1,

    /// <summary>
    /// Return value indicating that the current processor succeeded and subsequent processors should be skipped
    /// </summary>
    Abort = 2
}
