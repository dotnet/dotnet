// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel;

/// <summary>
/// The test process info payload.
/// </summary>
[DataContract]
public class TestProcessAttachDebuggerPayload
{
    /// <summary>
    /// Creates a new instance of this class.
    /// </summary>
    /// <param name="pid">The process id the debugger should attach to.</param>
    public TestProcessAttachDebuggerPayload(int pid)
    {
        ProcessID = pid;
    }

    /// <summary>
    /// The process id the debugger should attach to.
    /// </summary>
    [DataMember]
    public int ProcessID { get; set; }

    [DataMember]
    // Added in version 7.
    public string? TargetFramework { get; set; }
}

[DataContract]
// Added in version 7.
public class EditorAttachDebuggerPayload
{
    /// <summary>
    /// The process id the debugger should attach to.
    /// </summary>
    [DataMember]
    public int ProcessID { get; set; }

    [DataMember]
    public string? TargetFramework { get; set; }

    [DataMember]
    public ICollection<string>? Sources { get; set; }
}
