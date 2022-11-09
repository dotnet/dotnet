// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel;

public enum PlatformTraceLevel
{
    /// <summary>
    /// Output no tracing and debugging messages..
    /// </summary>
    Off = 0,

    /// <summary>
    /// Output error-handling messages.
    /// </summary>
    Error = 1,

    /// <summary>
    /// Output warnings and error-handling messages.
    /// </summary>
    Warning = 2,

    /// <summary>
    /// Output informational messages, warnings, and error-handling messages..
    /// </summary>
    Info = 3,

    /// <summary>
    /// Output all debugging and tracing messages..
    /// </summary>
    Verbose = 4
}
