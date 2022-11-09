// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.Versioning;
using NuGet.Frameworks;

namespace NuGet.Packaging
{
    public interface IPackageFile
    {
        /// <summary>
        /// Gets the full path of the file inside the package.
        /// </summary>
        string Path
        {
            get;
        }

        /// <summary>
        /// Gets the path that excludes the root folder (content/lib/tools) and framework folder (if present).
        /// </summary>
        /// <example>
        /// If a package has the Path as 'content\[net40]\scripts\jQuery.js', the EffectivePath 
        /// will be 'scripts\jQuery.js'.
        /// 
        /// If it is 'tools\init.ps1', the EffectivePath will be 'init.ps1'.
        /// </example>
        string EffectivePath
        {
            get;
        }

        /// <summary>
        /// FrameworkName object representing this package file's target framework. Deprecated. Must be null on net5.0 and greater.
        /// </summary>
        [Obsolete("Use NuGetFramework instead. This property will be null for any frameworks net5.0 or above.")]
        FrameworkName TargetFramework
        {
            get;
        }

        /// <summary>
        /// NuGetFramework object representing this package file's target framework. Use this instead of TargetFramework.
        /// </summary>
        NuGetFramework NuGetFramework
        {
            get;
        }

        DateTimeOffset LastWriteTime
        {
            get;
        }

        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "This might be expensive")]
        Stream GetStream();
    }
}
