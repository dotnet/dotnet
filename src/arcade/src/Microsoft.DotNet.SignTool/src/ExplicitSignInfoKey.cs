// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics;

namespace Microsoft.DotNet.SignTool
{
    /// <summary>
    /// Key used to identify file-specific signing information in FileSignInfo.
    /// Represents the entity being signed (file name, path, attributes) rather than just certificate correlation.
    /// Can be used to specify signing properties like DoNotUnpack independently of certificates.
    /// </summary>
    internal readonly struct ExplicitSignInfoKey : IEquatable<ExplicitSignInfoKey>
    {
        public readonly string FileName;
        public readonly string PublicKeyToken;
        public readonly string TargetFramework;
        public readonly string CollisionPriorityId;
        public readonly ExecutableType ExecutableType;

        public ExplicitSignInfoKey(string fileName, string publicKeyToken = null, string targetFramework = null, string collisionPriorityId = null, ExecutableType executableType = ExecutableType.None)
        {
            Debug.Assert(fileName != null);

            FileName = fileName;
            PublicKeyToken = publicKeyToken ?? "";
            TargetFramework = targetFramework ?? "";
            CollisionPriorityId = collisionPriorityId ?? "";
            ExecutableType = executableType;
        }

        public override bool Equals(object obj)
            => obj is ExplicitSignInfoKey key && Equals(key);

        public override int GetHashCode()
            => Hash.Combine(Hash.Combine(FileName.GetHashCode(), PublicKeyToken.GetHashCode()), Hash.Combine(TargetFramework.GetHashCode(), ExecutableType.GetHashCode()));

        bool IEquatable<ExplicitSignInfoKey>.Equals(ExplicitSignInfoKey other)
            => FileName == other.FileName && 
            CollisionPriorityId == other.CollisionPriorityId &&
            string.Equals(PublicKeyToken, other.PublicKeyToken, StringComparison.OrdinalIgnoreCase) && 
            TargetFramework == other.TargetFramework &&
            ExecutableType == other.ExecutableType;

        public static bool operator ==(ExplicitSignInfoKey key1, ExplicitSignInfoKey key2) 
            => key1.Equals(key2);

        public static bool operator !=(ExplicitSignInfoKey key1, ExplicitSignInfoKey key2)
            => !(key1 == key2);
    }
}
