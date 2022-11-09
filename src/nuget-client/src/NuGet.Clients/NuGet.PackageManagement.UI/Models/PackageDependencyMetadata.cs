// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Globalization;
using NuGet.Versioning;

namespace NuGet.PackageManagement.UI
{
    public class PackageDependencyMetadata
    {
        public PackageDependencyMetadata()
        {
        }

        public PackageDependencyMetadata(Packaging.Core.PackageDependency serverData)
        {
            Id = serverData.Id;
            Range = serverData.VersionRange;
        }

        public string Id { get; }

        public VersionRange Range { get; }

        public bool IsNoDependencyPlaceHolder { get; internal set; }

        public PackageDependencyMetadata(string id, VersionRange range)
        {
            Id = id;
            Range = range;
        }

        public override string ToString()
        {
            if (IsNoDependencyPlaceHolder)
            {
                return Resources.Text_NoDependencies;
            }

            if (Range == null)
            {
                return Id;
            }
            return string.Format(
                CultureInfo.InvariantCulture,
                "{0} {1}",
                Id, Range.PrettyPrint());
        }
    }
}
