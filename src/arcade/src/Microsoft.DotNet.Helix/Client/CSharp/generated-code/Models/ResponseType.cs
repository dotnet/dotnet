using System;
using System.Collections.Immutable;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Microsoft.DotNet.Helix.Client.Models
{
    public enum ResponseType
    {
        [EnumMember(Value = "Hits")]
        Hits,
        [EnumMember(Value = "HitsPerFile")]
        HitsPerFile,
    }
}
