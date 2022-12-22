// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Test.Utility.Signing
{
    // Only exists so as to avoid requiring NuGet.Packaging.Signing.X509StorePurpose from being public.
    public enum X509StorePurpose
    {
        CodeSigning = NuGet.Packaging.Signing.X509StorePurpose.CodeSigning,
        Timestamping = NuGet.Packaging.Signing.X509StorePurpose.Timestamping
    }
}
