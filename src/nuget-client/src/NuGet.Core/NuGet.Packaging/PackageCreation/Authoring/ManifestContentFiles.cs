// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.Packaging
{
    public class ManifestContentFiles
    {
        public string Include { get; set; }

        public string Exclude { get; set; }

        public string BuildAction { get; set; }

        public string CopyToOutput { get; set; }

        public string Flatten { get; set; }
    }
}
