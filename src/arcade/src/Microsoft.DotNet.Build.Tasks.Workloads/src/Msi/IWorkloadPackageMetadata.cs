// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable enable

using System;
using NuGet.Versioning;

namespace Microsoft.DotNet.Build.Tasks.Workloads.Msi
{
    /// <summary>
    /// Interface representing metadata associated with workload packages and used to generate other artifacts such as
    /// MSIs, SWIX packages for Visual Studio or payload packages for NuGet that wrap the workload MSIs.
    /// </summary>
    public interface IWorkloadPackageMetadata
    {
        public string Id
        {
            get;
        }

        public NuGetVersion PackageVersion
        {
            get;
        }

        public Version MsiVersion
        {
            get;
        }

        public string Authors
        {
            get;
        }

        public string Copyright
        {
            get;
        }

        public string Description
        {
            get;
        }

        public string Title
        {
            get;
        }

        public string? LicenseUrl
        {
            get;
        }
        public string ProjectUrl
        {
            get;
        }

        public string SwixPackageId
        {
            get;
        }        
    }
}

#nullable disable
