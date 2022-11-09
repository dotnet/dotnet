// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using NuGet.Common;
using static NuGet.Common.PathValidator;

namespace NuGet.PackageManagement.VisualStudio
{
    public static class PathValidator
    {
        public static string GetCanonicalPath(string path)
        {
            if (IsValidLocalPath(path)
                || (IsValidUncPath(path)))
            {
                return Path.GetFullPath(PathUtility.EnsureTrailingSlash(path));
            }
            if (IsValidUrl(path))
            {
                var url = new Uri(path);
                // return canonical representation of Uri
                return url.AbsoluteUri;
            }
            return path;
        }
    }
}
