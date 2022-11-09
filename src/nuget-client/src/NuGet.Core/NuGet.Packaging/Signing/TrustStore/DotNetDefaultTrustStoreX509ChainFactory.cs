// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Security.Cryptography.X509Certificates;

namespace NuGet.Packaging.Signing
{
    internal sealed class DotNetDefaultTrustStoreX509ChainFactory : IX509ChainFactory
    {
        public X509Chain Create()
        {
            return new X509Chain();
        }
    }
}
