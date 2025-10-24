// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics;

namespace Microsoft.DotNet.SignTool
{
    internal readonly struct ExplicitCertificateKey : IEquatable<ExplicitCertificateKey>
    {
        public readonly string FileName;
        public readonly string PublicKeyToken;
        public readonly string TargetFramework;
        public readonly string CollisionPriorityId;
        public readonly ExecutableType ExecutableType;

        public ExplicitCertificateKey(string fileName, string publicKeyToken = null, string targetFramework = null, string collisionPriorityId = null, ExecutableType executableType = ExecutableType.None)
        {
            Debug.Assert(fileName != null);

            FileName = fileName;
            PublicKeyToken = publicKeyToken ?? "";
            TargetFramework = targetFramework ?? "";
            CollisionPriorityId = collisionPriorityId ?? "";
            ExecutableType = executableType;
        }

        public override bool Equals(object obj)
            => obj is ExplicitCertificateKey key && Equals(key);

        public override int GetHashCode()
            => Hash.Combine(Hash.Combine(FileName.GetHashCode(), PublicKeyToken.GetHashCode()), Hash.Combine(TargetFramework.GetHashCode(), ExecutableType.GetHashCode()));

        bool IEquatable<ExplicitCertificateKey>.Equals(ExplicitCertificateKey other)
            => FileName == other.FileName && 
            CollisionPriorityId == other.CollisionPriorityId &&
            string.Equals(PublicKeyToken, other.PublicKeyToken, StringComparison.OrdinalIgnoreCase) && 
            TargetFramework == other.TargetFramework &&
            ExecutableType == other.ExecutableType;

        public static bool operator ==(ExplicitCertificateKey key1, ExplicitCertificateKey key2) 
            => key1.Equals(key2);

        public static bool operator !=(ExplicitCertificateKey key1, ExplicitCertificateKey key2)
            => !(key1 == key2);
    }
}
