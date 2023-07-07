// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Newtonsoft.Json.Linq;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal class RazorLanguageServerCapability : ICapabilitiesProvider
{
    private const string RazorCapabilityKey = "razor";
    private static readonly RazorLanguageServerCapability s_default = new RazorLanguageServerCapability
    {
        RangeMapping = true,
        BreakpointSpan = true,
        ProximityExpressions = true
    };

    public bool RangeMapping { get; set; }
    public bool BreakpointSpan { get; set; }
    public bool ProximityExpressions { get; set; }

    public void ApplyCapabilities(VSInternalServerCapabilities serverCapabilities, VSInternalClientCapabilities clientCapabilities)
    {
        serverCapabilities.Experimental ??= new Dictionary<string, object>();

        var dict = (Dictionary<string, object>)serverCapabilities.Experimental;
        dict["razor"] = s_default;
    }

    public static bool TryGet(JToken token, [NotNullWhen(true)] out RazorLanguageServerCapability? razorCapability)
    {
        if (token is not JObject jobject)
        {
            razorCapability = null;
            return false;
        }

        if (!jobject.TryGetValue("experimental", out var experimentalCapabilitiesToken))
        {
            razorCapability = null;
            return false;
        }

        if (experimentalCapabilitiesToken is not JObject experimentalCapabilities)
        {
            razorCapability = null;
            return false;
        }

        if (!experimentalCapabilities.TryGetValue(RazorCapabilityKey, out var razorCapabilityToken))
        {
            razorCapability = null;
            return false;
        }

        razorCapability = razorCapabilityToken.ToObject<RazorLanguageServerCapability>();
        return razorCapability is not null;
    }
}
