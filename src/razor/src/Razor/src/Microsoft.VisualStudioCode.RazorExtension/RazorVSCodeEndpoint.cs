// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.CodeAnalysis.ExternalAccess.Razor.Features;

namespace Microsoft.VisualStudioCode.RazorExtension;

internal class RazorVSCodeEndpoint : RazorEndpointAttribute
{
    public RazorVSCodeEndpoint(string method) : base(method, "Razor")
    {
    }
}
