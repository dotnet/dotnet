// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.Serialization;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;

/// <summary>
/// Class identifying a session.
/// </summary>
[DataContract]
public sealed class SessionId
{
    public SessionId()
    {
        Id = Guid.NewGuid();
    }

    public SessionId(Guid id)
    {
        Id = id;
    }

    [DataMember]
    public static SessionId Empty { get; } = new SessionId(Guid.Empty);

    [DataMember]
    public Guid Id { get; }

    public override bool Equals(object? obj) => obj is SessionId sessionId && Id.Equals(sessionId.Id);

    public override int GetHashCode() => Id.GetHashCode();

    public override string ToString() => Id.ToString("B");
}
