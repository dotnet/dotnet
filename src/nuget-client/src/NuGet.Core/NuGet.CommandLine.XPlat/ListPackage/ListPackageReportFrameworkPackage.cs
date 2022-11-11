// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using NuGet.Protocol;

namespace NuGet.CommandLine.XPlat.ListPackage
{
    /// <summary>
    /// Calculated project model data for a targetframework
    /// </summary>
    internal class ListPackageReportFrameworkPackage
    {
        internal string Framework { get; set; }
        internal List<ListReportPackage> TopLevelPackages { get; set; }
        internal List<ListReportPackage> TransitivePackages { get; set; }
        public ListPackageReportFrameworkPackage(string frameWork)
        {
            Framework = frameWork;
        }
    }
}
