// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NuGet.Common;

namespace NuGet.Packaging.Rules
{
    public class DefaultManifestValuesRule : IPackageRule
    {
        internal static readonly Uri SampleProjectUrl = new Uri("http://PROJECT_URL_HERE_OR_DELETE_THIS_LINE");
        internal static readonly Uri SampleLicenseUrl = new Uri("http://LICENSE_URL_HERE_OR_DELETE_THIS_LINE");
        internal static readonly Uri SampleIconUrl = new Uri("http://ICON_URL_HERE_OR_DELETE_THIS_LINE");
        internal const string SampleTags = "Tag1 Tag2";
        internal const string SampleReleaseNotes = "Summary of changes made in this release of the package.";
        internal const string SampleDescription = "Package description";
        internal const string SampleManifestDependencyId = "SampleDependency";
        internal const string SampleManifestDependencyVersion = "1.0";

        public string MessageFormat { get; }

        public DefaultManifestValuesRule(string messageFormat)
        {
            MessageFormat = messageFormat;
        }

        public IEnumerable<PackagingLogMessage> Validate(PackageArchiveReader builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            var nuspecReader = builder.NuspecReader;

            Uri.TryCreate(nuspecReader.GetProjectUrl(), UriKind.RelativeOrAbsolute, out var projectUrl);
            if (projectUrl == SampleProjectUrl)
            {
                yield return CreateIssueFor("projectUrl", nuspecReader.GetProjectUrl());
            }

            Uri.TryCreate(nuspecReader.GetLicenseUrl(), UriKind.RelativeOrAbsolute, out var licenseUrl);
            if (licenseUrl == SampleLicenseUrl)
            {
                yield return CreateIssueFor("licenseUrl", nuspecReader.GetLicenseUrl());
            }

            Uri.TryCreate(nuspecReader.GetIconUrl(), UriKind.RelativeOrAbsolute, out var iconUrl);
            if (iconUrl == SampleIconUrl)
            {
                yield return CreateIssueFor("iconUrl", nuspecReader.GetIconUrl());
            }

            if (SampleTags.Equals(nuspecReader.GetTags(), StringComparison.Ordinal))
            {
                yield return CreateIssueFor("tags", SampleTags);
            }

            if (SampleReleaseNotes.Equals(nuspecReader.GetReleaseNotes(), StringComparison.Ordinal))
            {
                yield return CreateIssueFor("releaseNotes", SampleReleaseNotes);
            }

            if (SampleDescription.Equals(nuspecReader.GetDescription(), StringComparison.Ordinal))
            {
                yield return CreateIssueFor("description", SampleDescription);
            }

            var dependency = nuspecReader.GetDependencyGroups().SelectMany(d => d.Packages).FirstOrDefault();
            if (dependency != null &&
                dependency.Id.Equals(SampleManifestDependencyId, StringComparison.OrdinalIgnoreCase) &&
                dependency.VersionRange != null &&
                dependency.VersionRange.ToString().Equals("[" + SampleManifestDependencyVersion + "]", StringComparison.OrdinalIgnoreCase))
            {
                yield return CreateIssueFor("dependency", dependency.ToString());
            }
        }

        private PackagingLogMessage CreateIssueFor(string field, string value)
        {
            return PackagingLogMessage.CreateWarning(
                string.Format(CultureInfo.CurrentCulture, MessageFormat, value, field),
                NuGetLogCode.NU5102);
        }
    }
}
