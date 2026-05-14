// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.Client.Interfaces;

public class AttachDebuggerInfo
{
    public int ProcessId { get; set; }
    public string? TargetFramework { get; set; }
    public ICollection<string>? Sources { get; set; }
}
