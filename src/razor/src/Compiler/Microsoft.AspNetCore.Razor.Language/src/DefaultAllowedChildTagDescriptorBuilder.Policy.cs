// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.AspNetCore.Razor.Language;

internal partial class DefaultAllowedChildTagDescriptorBuilder
{
    private sealed class Policy : TagHelperPooledObjectPolicy<DefaultAllowedChildTagDescriptorBuilder>
    {
        public static readonly Policy Instance = new();

        public override DefaultAllowedChildTagDescriptorBuilder Create() => new();

        public override bool Return(DefaultAllowedChildTagDescriptorBuilder builder)
        {
            builder._parent = null;

            builder.Name = null;
            builder.DisplayName = null;

            ClearDiagnostics(builder._diagnostics);

            return true;
        }
    }
}
