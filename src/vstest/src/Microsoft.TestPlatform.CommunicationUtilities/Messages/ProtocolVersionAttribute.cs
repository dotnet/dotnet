// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;

namespace Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;

[Conditional("DEBUG")]
[AttributeUsage(AttributeTargets.Field)]
internal class ProtocolVersionAttribute : Attribute
{
    public ProtocolVersionAttribute(int added, Type payloadType)
    {
        Added = added;
        PayloadType = payloadType;
    }

    public int Added { get; }
    public Type PayloadType { get; }
    public int Deprecated { get; set; }
    public string? Description { get; set; }
    public bool IsUsed { get; set; }
}
