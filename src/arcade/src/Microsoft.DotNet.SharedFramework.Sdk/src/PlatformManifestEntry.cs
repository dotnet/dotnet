// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.DotNet.SharedFramework.Sdk
{
    struct PlatformManifestEntry
    {
        public string Name { get; set; }
        public string AssemblyVersion { get; set; }
        public string FileVersion { get; set; }
        public bool IsNative { get; set; }
    }
}
