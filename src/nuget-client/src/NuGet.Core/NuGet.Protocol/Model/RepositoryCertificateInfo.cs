// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Newtonsoft.Json;
using NuGet.Packaging.Core;

namespace NuGet.Protocol
{
    public class RepositoryCertificateInfo : IRepositoryCertificateInfo
    {
        [JsonProperty(PropertyName = JsonProperties.Fingerprints)]
        public Fingerprints Fingerprints { get; private set; }

        [JsonProperty(PropertyName = JsonProperties.Subject)]
        public string Subject { get; private set; }

        [JsonProperty(PropertyName = JsonProperties.Issuer)]
        public string Issuer { get; private set; }

        [JsonProperty(PropertyName = JsonProperties.NotBefore)]
        public DateTimeOffset NotBefore { get; private set; }

        [JsonProperty(PropertyName = JsonProperties.NotAfter)]
        public DateTimeOffset NotAfter { get; private set; }

        [JsonProperty(PropertyName = JsonProperties.ContentUrl)]
        public string ContentUrl { get; private set; }
    }
}
