// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using NuGet.Common;
using NuGet.Packaging.Signing;

namespace NuGet.Packaging
{
    public class PackageExtractionContext
    {
        public ILogger Logger { get; }

        public PackageSaveMode PackageSaveMode { get; set; }

        public XmlDocFileSaveMode XmlDocFileSaveMode { get; set; }

        public ClientPolicyContext ClientPolicyContext { get; }

        public bool CopySatelliteFiles { get; set; } = true;

        /// <remarks>
        /// This property should only be used to override the default verifier on tests.
        /// It is public only so that NuGet.Commands.RestoreRequest can pass this property through
        /// </remarks>
        public IPackageSignatureVerifier SignedPackageVerifier { get; set; }

        public PackageExtractionContext(
            PackageSaveMode packageSaveMode,
            XmlDocFileSaveMode xmlDocFileSaveMode,
            ClientPolicyContext clientPolicyContext,
            ILogger logger)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            PackageSaveMode = packageSaveMode;
            XmlDocFileSaveMode = xmlDocFileSaveMode;
            ClientPolicyContext = clientPolicyContext;
        }
    }
}
