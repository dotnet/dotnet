// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Xml.Linq;

namespace NuGet.Commands
{
    public class MSBuildOutputFile
    {
        /// <summary>
        /// Output path on disk.
        /// </summary>
        public string Path { get; }

        /// <summary>
        /// MSBuild file content. This will be null for files
        /// that should be removed.
        /// </summary>
        public XDocument Content { get; }

        public MSBuildOutputFile(string path, XDocument content)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            Path = path;
            Content = content;
        }
    }
}
