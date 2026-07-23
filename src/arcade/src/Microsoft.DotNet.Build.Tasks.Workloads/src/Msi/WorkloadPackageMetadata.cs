// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable enable

using System;
using NuGet.Versioning;

namespace Microsoft.DotNet.Build.Tasks.Workloads.Msi
{
    internal class WorkloadPackageMetadata : IWorkloadPackageMetadata
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

        public WorkloadPackageMetadata(string id, NuGetVersion packageVersion, Version msiVersion, string authors, string copyright, string description, string title, string? licenseUrl, string projectUrl, string swixPackageId)
        {
            Id = id;
            PackageVersion = packageVersion;
            MsiVersion = msiVersion;
            Authors = authors;
            Copyright = copyright;
            Description = description;
            Title = title;
            LicenseUrl = licenseUrl;
            ProjectUrl = projectUrl;
            SwixPackageId = swixPackageId;
        }
    }
}

#nullable disable
