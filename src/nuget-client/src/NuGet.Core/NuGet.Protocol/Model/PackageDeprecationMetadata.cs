// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using Newtonsoft.Json;

namespace NuGet.Protocol
{
    public class PackageDeprecationMetadata
    {
        [JsonProperty(PropertyName = JsonProperties.DeprecationMessage)]
        public string Message { get; internal set; }

        [JsonProperty(PropertyName = JsonProperties.DeprecationReasons)]
        public IEnumerable<string> Reasons { get; internal set; }

        [JsonProperty(PropertyName = JsonProperties.AlternatePackage)]
        public AlternatePackageMetadata AlternatePackage { get; internal set; }
    }
}
