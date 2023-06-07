// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.AspNetCore.Razor.Language;

internal partial class DefaultRequiredAttributeDescriptorBuilder
{
    private sealed class Policy : TagHelperPooledObjectPolicy<DefaultRequiredAttributeDescriptorBuilder>
    {
        public static readonly Policy Instance = new();

        public override DefaultRequiredAttributeDescriptorBuilder Create() => new();

        public override bool Return(DefaultRequiredAttributeDescriptorBuilder builder)
        {
            builder._parent = null;

            builder.Name = null;
            builder.NameComparisonMode = default;
            builder.Value = null;
            builder.ValueComparisonMode = default;

            ClearDiagnostics(builder._diagnostics);

            builder._metadata.Clear();

            return true;
        }
    }
}
