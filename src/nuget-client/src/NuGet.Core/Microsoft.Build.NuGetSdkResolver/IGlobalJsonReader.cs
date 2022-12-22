// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using Microsoft.Build.Framework;

namespace Microsoft.Build.NuGetSdkResolver
{
    /// <summary>
    /// Represents an interface for a class that reads global.json.
    /// </summary>
    internal interface IGlobalJsonReader
    {
        /// <summary>
        /// Walks up the directory tree to find the first global.json and reads the msbuild-sdks section.
        /// </summary>
        /// <param name="context">An <see cref="SdkResolverContext" /> to use when locating the file.</param>
        /// <param name="fileName">An optional file name to search for, the default is global.json.</param>
        /// <returns>A <see cref="Dictionary{String,String}" /> of MSBuild SDK versions from a global.json if found, otherwise <c>null</c>.</returns>
        Dictionary<string, string> GetMSBuildSdkVersions(SdkResolverContext context, string fileName = GlobalJsonReader.GlobalJsonFileName);
    }
}
