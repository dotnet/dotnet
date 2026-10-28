// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;

namespace Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;

internal static class ProtocolVersioning
{
    public const int HighestSupportedVersion = Version7;
    public const int LowestSupportedVersion = Version0;

    // 0: the original protocol with no versioning (Message). It is used during negotiation.
    // 1: new protocol with versioning (VersionedMessage).
    // 2: changed serialization because the serialization of properties in bag was too verbose,
    //    so common properties are considered built-in and serialized without type info.
    // 3: introduced because of changes to allow attaching debugger to external process.
    // 4: introduced because 3 did not update this table and ended up using the serializer for protocol v1,
    //    which is extremely slow. We negotiate 2 or 4, but never 3 unless the flag above is set.
    // 5: ???
    // 6: accepts abort and cancel with handlers that report the status.
    /// <summary>
    /// The original protocol with no versioning. It sends and receives a Message that carries just data
    /// and has no Version field. It is used during negotiation to ensure that we communicate at the
    /// lowest available version the other side can be.
    /// </summary>
    public const int Version0 = 0;

    /// <summary>
    /// Adds versioning to the protocol by introducing VersionedMessage.
    /// </summary>
    public const int Version1 = 1;

    /// <summary>
    /// Changed serialization because the serialization of properties in bag was too verbose,
    /// so common properties are considered built-in and serialized without type info.
    /// </summary>
    public const int Version2 = 2;

    /// <summary>
    /// Added attach debugger messages to allow attaching to external process.
    /// /!\ This protocol version should not be used. It incorrectly serializes using the old
    /// version 1 serializer that is very slow.
    /// Added messages:
    /// <see cref="MessageType.AttachDebugger"/>
    /// <see cref="MessageType.AttachDebuggerCallback"/>
    /// <see cref="MessageType.EditorAttachDebugger"/>
    /// <see cref="MessageType.EditorAttachDebuggerCallback"/>
    /// </summary>
    public const int Version3 = 3;

    // 4: introduced because 3 did not update this table and ended up using the serializer for protocol v1,
    //    which is extremely slow. We negotiate 2 or 4, but never 3 unless the flag above is set.
    public const int Version4 = 4;
    public const int Version5 = 5;
    public const int Version6 = 6;
    public const int Version7 = 7;
}
