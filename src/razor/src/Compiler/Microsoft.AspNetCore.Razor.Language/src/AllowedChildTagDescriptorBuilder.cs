// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

namespace Microsoft.AspNetCore.Razor.Language;

public abstract class AllowedChildTagDescriptorBuilder
{
    public abstract string Name { get; set; }

    public abstract string DisplayName { get; set; }

    public abstract RazorDiagnosticCollection Diagnostics { get; }
}
