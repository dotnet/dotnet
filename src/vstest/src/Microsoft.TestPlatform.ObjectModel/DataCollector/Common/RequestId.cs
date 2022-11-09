// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;

/// <summary>
/// Wrapper class for a request ID that can be used for messages or events for identification
/// purposes
/// </summary>
[DataContract]
public sealed class RequestId : IEquatable<RequestId>, IComparable<RequestId>, IComparable
{
    /// <summary>
    /// A request ID with an empty GUID
    /// </summary>
    public static readonly RequestId Empty = new(Guid.Empty);

    /// <summary>
    /// Initializes the instance by creating a new GUID
    /// </summary>
    internal RequestId()
    {
        Id = Guid.NewGuid();
    }

    /// <summary>
    /// Initializes the instance with the provided GUID
    /// </summary>
    /// <param name="id">The GUID to use as the underlying ID</param>
    internal RequestId(Guid id)
    {
        Id = id;
    }


    #region Overrides

    /// <summary>
    /// Compares this instance with the provided object for value equality
    /// </summary>
    /// <param name="obj">The object to compare to</param>
    /// <returns>True if equal, false otherwise</returns>
    public override bool Equals(object? obj)
    {
        if (obj == null)
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        RequestId? other = obj as RequestId;
        return other != null && Id == other.Id;
    }

    /// <summary>
    /// Gets a hash code for this instance
    /// </summary>
    /// <returns>The underlying GUID's hash code</returns>
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    /// <summary>
    /// Converts the instance to a string in lower-case registry format
    /// </summary>
    /// <returns>A lower-case string in registry format representing the underlying GUID</returns>
    public override string ToString()
    {
        return Id.ToString("B");
    }

    #endregion

    #region Interface implementations

    #region IEquatable<RequestId> Members

    /// <summary>
    /// Compares this instance with the provided request ID for value equality
    /// </summary>
    /// <param name="other">The request ID to compare to</param>
    /// <returns>True if equal, false otherwise</returns>
    public bool Equals(RequestId? other)
    {
        return
            other != null && (
                ReferenceEquals(this, other) ||
                Id == other.Id
            );
    }

    #endregion

    #region IComparable<RequestId> Members

    /// <summary>
    /// Compares this instance with the provided request ID
    /// </summary>
    /// <param name="other">The request ID to compare to</param>
    /// <returns>An indication of the two request IDs' relative values</returns>
    public int CompareTo(RequestId? other)
    {
        return other == null ? 1 : Id.CompareTo(other.Id);
    }

    #endregion

    #region IComparable Members

    /// <summary>
    /// Compares this instance with the provided object
    /// </summary>
    /// <param name="obj">The object to compare to</param>
    /// <returns>An indication of the two objects' relative values</returns>
    /// <exception cref="ArgumentException">
    /// 'obj' is not null and not an instance of <see cref="RequestId"/>
    /// </exception>
    public int CompareTo(object? obj)
    {
        if (obj == null)
        {
            return 1;
        }

        RequestId? other = obj as RequestId;
        return other == null
            ? throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.Resources.Common_ObjectMustBeOfType, new object[] { typeof(RequestId).Name }), nameof(obj))
            : Id.CompareTo(other.Id);
    }

    #endregion

    #endregion

    #region Operators

    /// <summary>
    /// Compares the two request IDs for value equality
    /// </summary>
    /// <param name="left">The left-hand request ID</param>
    /// <param name="right">The right-hand request ID</param>
    /// <returns>True if equal, false otherwise</returns>
    public static bool operator ==(RequestId? left, RequestId? right)
    {
        return
            ReferenceEquals(left, right) ||
            left is not null &&
            right is not null &&
            left.Id == right.Id;
    }

    /// <summary>
    /// Compares two request IDs for value inequality
    /// </summary>
    /// <param name="left">The left-hand request ID</param>
    /// <param name="right">The right-hand request ID</param>
    /// <returns>True if unequal, false otherwise</returns>
    public static bool operator !=(RequestId? left, RequestId? right)
    {
        return !(left == right);
    }

    #endregion
    /// <summary>
    /// Gets the underlying GUID that represents the request ID
    /// </summary>
    [DataMember]
    public Guid Id
    {
        get;
        private set;
    }

}
