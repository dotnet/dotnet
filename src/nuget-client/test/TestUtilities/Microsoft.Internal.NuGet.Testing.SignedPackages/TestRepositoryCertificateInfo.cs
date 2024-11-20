// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#pragma warning disable CS1591

using System;
using NuGet.Packaging.Core;

namespace Microsoft.Internal.NuGet.Testing.SignedPackages
{
    public class TestRepositoryCertificateInfo : IRepositoryCertificateInfo
    {
        public string ContentUrl { get; set; }

        public Fingerprints Fingerprints { get; set; }

        public string Issuer { get; set; }

        public DateTimeOffset NotAfter { get; set; }

        public DateTimeOffset NotBefore { get; set; }

        public string Subject { get; set; }
    }
}
