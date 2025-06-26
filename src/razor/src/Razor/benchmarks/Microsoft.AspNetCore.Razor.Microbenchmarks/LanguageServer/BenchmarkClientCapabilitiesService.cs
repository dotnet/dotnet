// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using Microsoft.CodeAnalysis.Razor.Protocol;

namespace Microsoft.AspNetCore.Razor.Microbenchmarks.LanguageServer;

internal class BenchmarkClientCapabilitiesService(VSInternalClientCapabilities capabilities) : IClientCapabilitiesService
{
    public bool CanGetClientCapabilities => true;

    public VSInternalClientCapabilities ClientCapabilities => capabilities;
}
