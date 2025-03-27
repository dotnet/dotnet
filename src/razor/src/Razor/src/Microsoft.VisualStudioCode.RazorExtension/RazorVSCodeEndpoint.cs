// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.ExternalAccess.Razor.Features;

namespace Microsoft.VisualStudioCode.RazorExtension;

internal class RazorVSCodeEndpoint : RazorEndpointAttribute
{
    public RazorVSCodeEndpoint(string method) : base(method, "Razor")
    {
    }
}
