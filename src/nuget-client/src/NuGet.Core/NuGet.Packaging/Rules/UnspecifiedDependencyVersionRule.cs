// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NuGet.Common;
using NuGet.Versioning;

namespace NuGet.Packaging.Rules
{
    public class UnspecifiedDependencyVersionRule : IPackageRule
    {
        public string MessageFormat { get; }

        public UnspecifiedDependencyVersionRule(string messageFormat)
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

            var dependency = nuspecReader.GetDependencyGroups().SelectMany(d => d.Packages).FirstOrDefault();

            if (dependency != null && dependency.VersionRange == VersionRange.All)
            {
                var issue = PackagingLogMessage.CreateWarning(string.Format(
                    CultureInfo.CurrentCulture,
                    AnalysisResources.UnspecifiedDependencyVersionWarning,
                    dependency.Id),
                    NuGetLogCode.NU5112);
                yield return issue;
            }
        }
    }
}
