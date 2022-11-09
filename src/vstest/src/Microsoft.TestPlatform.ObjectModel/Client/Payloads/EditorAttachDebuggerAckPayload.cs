// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.Serialization;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;

/// <summary>
/// Class used to define the <see cref="EditorAttachDebuggerAckPayload"/> sent by the
/// vstest.console translation layers into design mode.
/// </summary>
public class EditorAttachDebuggerAckPayload
{
    /// <summary>
    /// A value indicating if the debugger has successfully attached.
    /// </summary>
    [DataMember]
    public bool Attached { get; set; }

    /// <summary>
    /// ErrorMessage, in cases where attaching the debugger fails.
    /// </summary>
    [DataMember]
    public string? ErrorMessage { get; set; }
}
