// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using NuGet.Packaging;

namespace NuGet.ProjectModel
{
    /// <summary>
    /// A Fowler-Noll-Vo (FNV) 64-bit hash function that supports non-cryptographic hashing to optimize speed and minimize collisions.
    /// https://en.wikipedia.org/wiki/Fowler%E2%80%93Noll%E2%80%93Vo_hash_function
    /// </summary>
    internal sealed class FnvHash64Function : IHashFunction
    {
        private ulong _hash;

        public void Update(byte[] data, int offset, int count)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            if (_hash == 0)
            {
                _hash = FnvHash64.Hash(data, count);
            }
            else
            {
                _hash = FnvHash64.Update(_hash, data, count);
            }
        }

        public byte[] GetHashBytes()
        {
            return BitConverter.GetBytes(_hash);
        }

        public string GetHash()
        {
            return Convert.ToBase64String(GetHashBytes());
        }

        // Interface is Disposable for SHA512 - this class has nothing to dispose.
        public void Dispose() { }

        internal static class FnvHash64
        {
            public const ulong Offset = 14695981039346656037;
            public const ulong Prime = 1099511628211;

            public static ulong Hash(byte[] data, int count = 0)
            {
                ulong hash = Offset;

                if (count == 0)
                {
                    count = data.Length;
                }

                unchecked
                {
                    for (int i = 0; i < count; i++)
                    {
                        var b = data[i];
                        hash = (hash ^ b) * Prime;
                    }
                }

                return hash;
            }

            public static ulong Combine(ulong left, ulong right)
            {
                unchecked
                {
                    return (left ^ right) * Prime;
                }
            }

            public static ulong Update(ulong current, byte[] data, int count = 0)
            {
                return Combine(current, Hash(data, count));
            }
        }
    }
}
