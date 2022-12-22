// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.Test.Apex.VisualStudio;

namespace NuGet.Tests.Apex
{
    public class NuGetApexVerifier : VisualStudioMarshallableProxyVerifier
    {
        /// <summary>
        /// Gets the Nuget Package Manager test service
        /// </summary>
        private NuGetApexTestService NugetPackageManager => (NuGetApexTestService)Owner;
    }
}
