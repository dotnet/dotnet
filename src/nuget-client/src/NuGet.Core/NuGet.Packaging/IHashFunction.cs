// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace NuGet.Packaging
{
    /// <summary>
    /// Provides incremental hashing.
    /// 
    /// This is non-private only to facilitate unit testing.
    /// </summary>
    public interface IHashFunction : IDisposable
    {
        /// <summary>
        /// Gets the hash.
        ///
        /// Once GetHash is called, no further hash updates are allowed.
        /// </summary>
        /// <returns>A base64-encoded hash.</returns>
        string GetHash();

        /// <summary>
        /// Incrementally updates the hash.
        /// </summary>
        /// <param name="data">The data to be included in the hash.</param>
        /// <param name="offset">The offset from which data should be read.</param>
        /// <param name="count">The count of bytes to read.</param>
        void Update(byte[] data, int offset, int count);
    }
}
