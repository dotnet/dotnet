// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable enable

using System;
using NuGet.Versioning;

namespace Microsoft.DotNet.Build.Tasks.Workloads.Msi
{
    internal class MsiMetadata
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

        public string LicenseUrl
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

        public MsiMetadata(string id, NuGetVersion packageVersion, Version msiVersion, string authors, string copyright, string description, string title, string licenseUrl, string projectUrl, string swixPackageId)
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

        public static MsiMetadata Create(WorkloadPackageBase package)
        {
            return new(
                package.Id,
                package.PackageVersion,
                package.MsiVersion,
                package.Authors,
                package.Copyright,
                package.Description,
                package.Title,
                package.LicenseUrl,
                package.ProjectUrl,
                package.SwixPackageId
                );
        }
    }
}

#nullable disable
