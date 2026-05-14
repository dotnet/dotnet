// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Security.Cryptography.X509Certificates;

namespace NuGet.Packaging.Signing
{
    internal sealed class DotNetDefaultTrustStoreX509ChainFactory : IX509ChainFactory
    {
        public IX509Chain Create()
        {
            return new X509ChainWrapper(new X509Chain());
        }
    }
}
