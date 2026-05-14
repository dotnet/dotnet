// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

namespace Internal.Microsoft.Extensions.DependencyModel
{
    internal class ResourceAssembly
    {
        public ResourceAssembly(string path, string locale)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException(nameof(path));
            }
            if (string.IsNullOrEmpty(locale))
            {
                throw new ArgumentException(nameof(locale));
            }
            Locale = locale;
            Path = path;
        }

        public string Locale { get; set; }

        public string Path { get; set; }

    }
}
