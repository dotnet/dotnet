// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using NuGet.Packaging.Core;
using NuGet.Versioning;

namespace NuGet.ProjectManagement
{
    public static class Constants
    {
        public static readonly string BinDirectory = "bin";
        public static readonly string PackageReferenceFile = "packages.config";
        public static readonly string MirroringReferenceFile = "mirroring.config";
        public static readonly string ReadmeFileName = "readme.txt";

        public static readonly string BeginIgnoreMarker = "NUGET: BEGIN LICENSE TEXT";
        public static readonly string EndIgnoreMarker = "NUGET: END LICENSE TEXT";

        internal const string PackageRelationshipNamespace = "http://schemas.microsoft.com/packaging/2010/07/";

        // This is temporary until we fix the gallery to have proper first class support for this.
        // The magic unpublished date is 1900-01-01T00:00:00
        public static readonly DateTimeOffset Unpublished = new DateTimeOffset(1900, 1, 1, 0, 0, 0, TimeSpan.FromHours(-8));

        [SuppressMessage(
            "Microsoft.Security",
            "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes",
            Justification = "The type is immutable.")]
        public static readonly ICollection<string> AssemblyReferencesExtensions
            = new ReadOnlyCollection<string>(new[] { ".dll", ".exe", ".winmd" });

        public const string ResourceAssemblyExtension = ".resources.dll";

        public static readonly string NativeTFM = "Native, Version=0.0";
        public static readonly string JSProjectExt = ".jsproj";
        public static readonly string VCXProjextExt = ".vcxproj";
        public static readonly string ProjectExt = "ProjectExt";
        public static readonly string TargetPlatformIdentifier = "TargetPlatformIdentifier";
        public static readonly string TargetPlatformVersion = "TargetPlatformVersion";
        public static readonly string TargetFrameworkMoniker = "TargetFrameworkMoniker";
    }
}
