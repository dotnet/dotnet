// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Newtonsoft.Json;
using System.Collections.Generic;

namespace Microsoft.DotNet.GitSync
{
    public class Configuration
    {
        public List<RepositoryInfo> Repos{ get; set; }
        public string RepositoryBasePath { get; set; }
        public string UserName { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string SecretUri { get; set; }
        [JsonIgnore]
        public string Password { get; set; }
        public string MirrorSignatureUser { get; set; }
        public List<string> Branches { get; set; }
        public string ConnectionString { get; set; }
        public string Server { get; set; }
        public string Destinations { get; set; }
    }
}
