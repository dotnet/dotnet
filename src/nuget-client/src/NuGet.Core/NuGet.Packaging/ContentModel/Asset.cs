// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace NuGet.ContentModel
{
    public class Asset
    {
        public required string Path { get; set; }

        [Obsolete("This is unused and will be removed in a future release.")]
        public string? Link { get; set; }

        public override string? ToString()
        {
            return Path;
        }
    }
}
