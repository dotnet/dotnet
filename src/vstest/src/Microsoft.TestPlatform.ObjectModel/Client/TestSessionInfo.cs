// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.Serialization;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;

/// <summary>
/// Defines the test session info object to be passed around between vstest.console and
/// vstest.console wrapper in order to identify the current session.
/// </summary>
[DataContract]
public class TestSessionInfo : IEquatable<TestSessionInfo>
{
    /// <summary>
    /// Creates an instance of the current class.
    /// </summary>
    public TestSessionInfo()
    {
        Id = Guid.NewGuid();
    }

    /// <summary>
    /// Gets the session id.
    /// </summary>
    [DataMember]
    public Guid Id { get; private set; }

    /// <summary>
    /// Calculates the hash code for the current object.
    /// </summary>
    ///
    /// <returns>An integer representing the computed hashcode value.</returns>
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    /// <summary>
    /// Checks if the specified object is equal to the current instance.
    /// </summary>
    ///
    /// <param name="obj">The object to be checked.</param>
    ///
    /// <returns>True if the two objects are equal, false otherwise.</returns>
    public override bool Equals(object? obj)
    {
        return Equals(obj as TestSessionInfo);
    }

    /// <summary>
    /// Checks if the specified session is equal to the current instance.
    /// </summary>
    ///
    /// <param name="other">The session to be checked.</param>
    ///
    /// <returns>True if the two sessions are equal, false otherwise.</returns>
    public bool Equals(TestSessionInfo? other)
    {
        return other != null && Id.Equals(other.Id);
    }
}
