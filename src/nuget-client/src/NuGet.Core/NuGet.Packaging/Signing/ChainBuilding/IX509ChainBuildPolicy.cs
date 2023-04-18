// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Security.Cryptography.X509Certificates;

namespace NuGet.Packaging.Signing
{
    /// <summary>
    /// Represents a wrapper around <see cref="X509Chain.Build(X509Certificate2)" /> to enable
    /// custom behaviors (e.g.:  retry on failure).
    /// </summary>
    internal interface IX509ChainBuildPolicy
    {
        bool Build(IX509Chain chain, X509Certificate2 certificate);
    }
}
