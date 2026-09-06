// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Text;

using Microsoft.TestPlatform.Hashing;

namespace Microsoft.TestPlatform.AdapterUtilities;

/// <summary>
/// Used to generate id for tests, using xxHash128.
/// </summary>
/// <remarks>
/// This is the intended successor to <see cref="TestIdProvider"/>, which uses SHA1. SHA1 is a
/// cryptographic hash being used for a non-cryptographic purpose; it is slower than necessary and
/// its presence trips security tooling. The ids produced here are RFC 9562 version 8 UUIDs that
/// carry the version of the hashing scheme, so a future change to the algorithm is detectable
/// from the id itself. It ships available but not default: <see cref="TestIdProvider"/> is still
/// what test ids are computed with unless a run selects otherwise.
/// </remarks>
public class TestIdProviderXxHash128
{
    private Guid _id = Guid.Empty;
    private byte[]? _hash;

    private readonly XxHash128 _hasher;

    /// <summary>
    /// Initializes a new instance of the <see cref="TestIdProviderXxHash128"/> class.
    /// </summary>
    public TestIdProviderXxHash128()
    {
        _hasher = new XxHash128();
    }

    /// <summary>
    /// Appends a string to id generation seed.
    /// </summary>
    /// <param name="str">String to append to the id seed.</param>
    /// <exception cref="InvalidOperationException">Thrown if <see cref="GetHash"/> or <see cref="GetId"/> is called already.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="str"/> is <see langword="null"/>.</exception>
    public void AppendString(string str)
    {
        if (_hash != null)
        {
            throw new InvalidOperationException(Resources.Resources.ErrorCannotAppendAfterHashCalculation);
        }
        _ = str ?? throw new ArgumentNullException(nameof(str));

        byte[] bytes = Encoding.Unicode.GetBytes(str);

        _hasher.Append(bytes);
    }

    /// <summary>
    /// Appends an array of bytes to id generation seed.
    /// </summary>
    /// <param name="bytes">Array to append to the id seed.</param>
    /// <exception cref="InvalidOperationException">Thrown if <see cref="GetHash"/> or <see cref="GetId"/> is called already.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="bytes"/> is <see langword="null"/>.</exception>
    public void AppendBytes(byte[] bytes)
    {
        if (_hash != null)
        {
            throw new InvalidOperationException(Resources.Resources.ErrorCannotAppendAfterHashCalculation);
        }
        _ = bytes ?? throw new ArgumentNullException(nameof(bytes));

        if (bytes.Length == 0)
        {
            return;
        }

        _hasher.Append(bytes);
    }

    /// <summary>
    /// Calculates the Id seed.
    /// </summary>
    /// <returns>An array containing the seed.</returns>
    /// <remarks>
    /// <see cref="AppendBytes(byte[])"/> and <see cref="AppendString(string)"/> cannot be called
    /// on instance after this method is called. Unlike <see cref="TestIdProvider.GetHash"/>, which
    /// hands out the array it caches, the returned array is a copy, so mutating it does not disturb
    /// a later <see cref="GetId"/> or <see cref="GetHash"/> call.
    /// </remarks>
    public byte[] GetHash()
    {
        return (byte[])EnsureHash().Clone();
    }

    /// <summary>
    /// Calculates the Id from the seed.
    /// </summary>
    /// <returns>Id</returns>
    /// <remarks>
    /// <see cref="AppendBytes(byte[])"/> and <see cref="AppendString(string)"/> cannot be called
    /// on instance after this method is called.
    /// </remarks>
    public Guid GetId()
    {
        if (_id != Guid.Empty)
        {
            return _id;
        }

        // VersionedGuidFromHash mutates what it is given, so hand it a copy and keep the cached
        // hash intact for callers that ask for it themselves.
        byte[] hash = (byte[])EnsureHash().Clone();
        _id = TestIdGuid.VersionedGuidFromHash(hash, TestIdGuid.CurrentHashVersion);

        return _id;
    }

    private byte[] EnsureHash() => _hash ??= _hasher.GetCurrentHash();
}
