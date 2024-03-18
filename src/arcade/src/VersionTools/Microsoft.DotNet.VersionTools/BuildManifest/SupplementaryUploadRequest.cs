// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.DotNet.VersionTools.BuildManifest
{
    public class SupplementaryUploadRequest
    {
        /// <summary>
        /// Path, relative to the primary upload dir or absolute with a leading '/'.
        /// </summary>
        public string Path { get; set; }

        public string Contents { get; set; }

        /// <summary>
        /// Combine currentPath and Path into the absolute path of this request. The result is
        /// compatible with GitHub paths. This is similar to Path.Combine except:
        /// 
        /// If Path is absolute (begins with '/'), the leading '/' is trimmed from the result.
        /// 
        /// If Path is relative, the path is always joined with '/'. (Never '\'.)
        /// </summary>
        /// <param name="currentPath">
        /// The absolute path of a dir that Path should be made relative to, if Path isn't already
        /// an absolute path. Can't start or end in '/'.
        /// </param>
        public string GetAbsolutePath(string currentPath)
        {
            if (Path.StartsWith("/"))
            {
                return Path.Substring(1);
            }
            return $"{currentPath}/{Path}";
        }
    }
}
