// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#pragma warning disable CS1591

namespace Microsoft.Internal.NuGet.Testing.SignedPackages
{
    // Only exists so as to avoid requiring NuGet.Packaging.Signing.X509StorePurpose from being public.
    public enum X509StorePurpose
    {
        CodeSigning = global::NuGet.Packaging.Signing.X509StorePurpose.CodeSigning,
        Timestamping = global::NuGet.Packaging.Signing.X509StorePurpose.Timestamping
    }
}
