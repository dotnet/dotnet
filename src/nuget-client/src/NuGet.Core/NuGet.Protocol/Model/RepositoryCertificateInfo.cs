// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Newtonsoft.Json;
using NuGet.Packaging.Core;

namespace NuGet.Protocol
{
    public class RepositoryCertificateInfo : IRepositoryCertificateInfo
    {
        [JsonProperty(PropertyName = JsonProperties.Fingerprints, Required = Required.Always)]
        public Fingerprints Fingerprints { get; private set; } = null!;

        [JsonProperty(PropertyName = JsonProperties.Subject, Required = Required.Always)]
        public string Subject { get; private set; } = null!;

        [JsonProperty(PropertyName = JsonProperties.Issuer, Required = Required.Always)]
        public string Issuer { get; private set; } = null!;

        [JsonProperty(PropertyName = JsonProperties.NotBefore, Required = Required.Always)]
        public DateTimeOffset NotBefore { get; private set; }

        [JsonProperty(PropertyName = JsonProperties.NotAfter, Required = Required.Always)]
        public DateTimeOffset NotAfter { get; private set; }

        [JsonProperty(PropertyName = JsonProperties.ContentUrl, Required = Required.Always)]
        public string ContentUrl { get; private set; } = null!;
    }
}
