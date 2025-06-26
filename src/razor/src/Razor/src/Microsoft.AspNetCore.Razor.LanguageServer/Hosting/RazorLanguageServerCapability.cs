// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Hosting;

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

    public static bool TryGet(JsonElement token, [NotNullWhen(true)] out RazorLanguageServerCapability? razorCapability)
    {
        if (token.ValueKind != JsonValueKind.Object)
        {
            razorCapability = null;
            return false;
        }

        if (!token.TryGetProperty("experimental", out var experimentalCapabilities))
        {
            razorCapability = null;
            return false;
        }

        if (!experimentalCapabilities.TryGetProperty(RazorCapabilityKey, out var razorCapabilityToken))
        {
            razorCapability = null;
            return false;
        }

        razorCapability = razorCapabilityToken.Deserialize<RazorLanguageServerCapability>();
        return razorCapability is not null;
    }
}
