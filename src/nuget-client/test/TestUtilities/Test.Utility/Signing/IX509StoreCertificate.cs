// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Security.Cryptography.X509Certificates;

namespace Test.Utility.Signing
{
    /// <summary>
    /// Represents an X.509 certificate in a specific X.509 store.  No trust is implied.
    /// </summary>
    public interface IX509StoreCertificate
    {
        StoreLocation StoreLocation { get; }
        StoreName StoreName { get; }
        X509Certificate2 Certificate { get; }
    }
}
