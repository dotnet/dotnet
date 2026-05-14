// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.Text;

namespace Microsoft.DotNet.Build.Tasks.Workloads.Swix
{
    /// <summary>
    /// A base class to create SWIX projects for Visual Studio setup packages.
    /// </summary>
    public abstract class SwixProjectBase : ProjectTemplateBase
    {
        /// <summary>
        /// The maximum relative path length of a package. The length accounts for the Visual Studio package cache
        /// created at install time.
        /// </summary>
        public const int MaxRelativePackagePath = 182;

        /// <summary>
        /// The ID of the SWIX package in the Visual Studio setup catalog. The ID is used
        /// to reference the package inside other setup packages such as components and component groups.
        /// </summary>
        public string Id
        {
            get;
            set;
        }

        /// <summary>
        /// Determines whether the package is marked as out-of-support.
        /// </summary>
        public bool OutOfSupport
        {
            get;
            set;
        }

        /// <summary>
        /// The version of the SWIX package.
        /// </summary>
        public Version Version
        {
            get;
            set;
        }

        /// <summary>
        /// The root directory for SWIX projects.
        /// </summary>
        protected string SwixDirectory => Path.Combine(SourceDirectory, "swix");

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">The SWIX package ID.</param>
        /// <param name="version">The package version.</param>
        public SwixProjectBase(string id, Version version, string baseIntermediateOutputPath, string baseOutputPath, bool outOfSupport = false) :
            base(baseIntermediateOutputPath, baseOutputPath)
        {
            Id = id;
            Version = version;
            OutOfSupport = outOfSupport;

            ReplacementTokens[SwixTokens.__VS_PACKAGE_NAME__] = Id;
            ReplacementTokens[SwixTokens.__VS_PACKAGE_VERSION__] = $"{Version}";
        }

        internal SwixProjectBase(SwixPackageBase package, string baseIntermediateOutputPath, string baseOutputPath, bool outOfSupport = false) :
            base(baseIntermediateOutputPath, baseOutputPath)
        {
            Id = package.Name;
            Version = package.Version;
            OutOfSupport = outOfSupport;

            ReplacementTokens[SwixTokens.__VS_PACKAGE_NAME__] = Id;
            ReplacementTokens[SwixTokens.__VS_PACKAGE_VERSION__] = $"{Version}";
        }


        /// <summary>
        /// Replace all tokens in the specified file.
        /// </summary>
        /// <param name="path">The path of the file to update.</param>
        protected void ReplaceTokens(string path)
        {
            Utils.StringReplace(path, ReplacementTokens, Encoding.UTF8);
        }

        /// <summary>
        /// Compute the relative path of the package within the Visual Studio package cache.
        /// </summary>
        /// <returns>The relative path of the package.</returns>
        protected virtual string GetRelativePackagePath() => $"{Id},version={Version}";

        /// <summary>
        /// Validates that the length of the relative package path does not execeed the maximum limit allowed by Visual Studio. The length
        /// accounts for the location of the Visual Studio installer package cache.
        /// </summary>
        /// <exception cref="Exception" />
        internal static void ValidateRelativePackagePath(string relativePackagePath)
        {
            _ = relativePackagePath ?? throw new ArgumentNullException(nameof(relativePackagePath));

            // Visual Studio will verify this as part of its manifest validation logic during PR builds, but
            // any error would require rebuilding workloads and effectively reset .NET builds. 
            if (relativePackagePath.Length > MaxRelativePackagePath)
            {
                throw new Exception(string.Format(Strings.RelativePackagePathTooLong, MaxRelativePackagePath, relativePackagePath));
            }
        }
    }
}
