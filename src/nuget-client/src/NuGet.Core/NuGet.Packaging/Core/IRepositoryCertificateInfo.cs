// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace NuGet.Packaging.Core
{
    public interface IRepositoryCertificateInfo
    {
        string ContentUrl { get; }

        Fingerprints Fingerprints { get; }

        string Issuer { get; }

        DateTimeOffset NotAfter { get; }

        DateTimeOffset NotBefore { get; }

        string Subject { get; }
    }
}
