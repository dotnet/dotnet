// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.Packaging.Signing
{
    /// <summary>
    /// A package that can read and write signatures.
    /// </summary>
    public interface ISignedPackage : ISignedPackageReader, ISignedPackageWriter
    {
        // This provides a combination of ISignPackageReader and IPackageSignatureWriter.
        // No additional methods are needed, but new functionality could be added here.
    }
}
