// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;

namespace NuGet.CommandLine.XPlat.ListPackage
{
    /// <summary>
    /// Calculated project model data for a targetframework
    /// </summary>
    internal class ListPackageReportFrameworkPackage
    {
        internal string Framework { get; }
        internal string TargetAlias { get; }
        internal List<ListReportPackage> TopLevelPackages { get; set; }
        internal List<ListReportPackage> TransitivePackages { get; set; }
        public ListPackageReportFrameworkPackage(string framework, string targetAlias)
        {
            Framework = framework ?? throw new ArgumentNullException(nameof(framework));
            TargetAlias = targetAlias ?? throw new ArgumentNullException(nameof(targetAlias));
        }
    }
}
