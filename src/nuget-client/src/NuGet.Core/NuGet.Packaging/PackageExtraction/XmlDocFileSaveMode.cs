// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.Packaging
{
    public enum XmlDocFileSaveMode
    {
        /// <summary>
        /// Treat XML doc files as regular files (legacy behavior).
        /// </summary>
        None,

        /// <summary>
        /// Do not extract XML documentation files
        /// </summary>
        Skip,

        /// <summary>
        /// Compress XML doc files in a zip archive.
        /// </summary>
        Compress,
    }
}
