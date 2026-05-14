// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Newtonsoft.Json;

namespace Microsoft.DotNet.Build.Tasks.VisualStudio
{
    internal sealed class OptProfInstrumentationArgument
    {
        [JsonProperty(PropertyName = "relativeInstallationFolder", Order = 3)]
        public string RelativeInstallationFolder { get; set; }

        [JsonProperty(PropertyName = "instrumentationExecutable", Order = 3)]
        public string InstrumentationExecutable { get; set; }
    }
}
