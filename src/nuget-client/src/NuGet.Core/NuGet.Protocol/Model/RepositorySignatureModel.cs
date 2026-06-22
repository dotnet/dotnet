// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Text.Json.Serialization;

namespace NuGet.Protocol.Model
{
    internal sealed class RepositorySignatureModel
    {
        [JsonPropertyName(JsonProperties.AllRepositorySigned)]
        public bool? AllRepositorySigned { get; set; }

        [JsonPropertyName(JsonProperties.SigningCertificates)]
        public RepositoryCertificateInfo[]? SigningCertificates { get; set; }
    }
}
