// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace Microsoft.DotNet.Build.Tasks.Packaging
{
    internal class PackageMetadata
    {
        public string Name = null;  // Remove warnings
        public string Description = null;
        public string[] CommonTypes = null;

        public static IEnumerable<PackageMetadata> ReadFrom(string path)
        {
            string packageMetadata = File.ReadAllText(path);

            return JsonConvert.DeserializeObject<List<PackageMetadata>>(packageMetadata);
        }
    }
}
