// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Newtonsoft.Json;
using System.IO;

namespace Microsoft.DotNet.Build.Tasks.VisualStudio
{
    internal sealed class OptProfTrainingConfiguration
    {
        [JsonProperty(PropertyName = "products")]
        public ProductOptProfTraining[] Products { get; set; }

        [JsonProperty(PropertyName = "assemblies")]
        public AssemblyOptProfTraining[] Assemblies { get; set; }

        public static OptProfTrainingConfiguration Deserialize(string json)
            => JsonSerializer.CreateDefault().Deserialize<OptProfTrainingConfiguration>(new StringReader(json));
    }
}
