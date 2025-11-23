// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable disable

namespace NuGet.ContentModel
{
    public class Asset
    {
        public string Path { get; set; }
        public string Link { get; set; }

        public override string ToString()
        {
            return Path;
        }
    }
}
