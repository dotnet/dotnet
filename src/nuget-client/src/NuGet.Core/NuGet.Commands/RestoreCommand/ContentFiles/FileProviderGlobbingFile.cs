// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.FileSystemGlobbing.Abstractions;

// This file is based on
// https://github.com/aspnet/Mvc/blob/dev/src/Microsoft.AspNet.Mvc.TagHelpers/Internal/FileProviderGlobbingFile.cs

namespace NuGet.Commands
{
    internal class FileProviderGlobbingFile : FileInfoBase
    {
        private const char DirectorySeparatorChar = '/';

        public FileProviderGlobbingFile(IFileInfo fileInfo, DirectoryInfoBase parent)
        {
            if (fileInfo == null)
            {
                throw new ArgumentNullException(nameof(fileInfo));
            }

            if (parent == null)
            {
                throw new ArgumentNullException(nameof(parent));
            }

            Name = fileInfo.Name;
            ParentDirectory = parent;

            FullName = ParentDirectory == null
                ? string.Empty
                : ParentDirectory.FullName + DirectorySeparatorChar + Name;
        }

        public override string FullName { get; }

        public override string Name { get; }

        public override DirectoryInfoBase ParentDirectory { get; }
    }
}
