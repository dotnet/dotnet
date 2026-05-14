// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace NuGet.Packaging.Signing
{
    /// <summary>
    /// Represents a certificate chain ordered from end certificate (index 0) to root certificate.
    /// </summary>
    public interface IX509CertificateChain : IReadOnlyList<X509Certificate2>, IDisposable
    {
    }
}
