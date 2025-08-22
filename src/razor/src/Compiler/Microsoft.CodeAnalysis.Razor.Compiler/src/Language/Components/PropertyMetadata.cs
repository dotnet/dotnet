// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Razor.Utilities;

namespace Microsoft.AspNetCore.Razor.Language.Components;

public sealed class PropertyMetadata() : MetadataObject(MetadataKind.Property)
{
    public string? GloballyQualifiedTypeName { get; init; }
    public bool IsChildContent { get; init; }
    public bool IsEventCallback { get; init; }
    public bool IsDelegateSignature { get; init; }
    public bool IsDelegateWithAwaitableResult { get; init; }
    public bool IsGenericTyped { get; init; }
    public bool IsInitOnlyProperty { get; init; }

    private protected override void BuildChecksum(in Checksum.Builder builder)
    {
        builder.AppendData(GloballyQualifiedTypeName);
    }

    public ref struct Builder
    {
        public string? GloballyQualifiedTypeName { get; set; }
        public bool IsChildContent { get; set; }
        public bool IsEventCallback { get; set; }
        public bool IsDelegateSignature { get; set; }
        public bool IsDelegateWithAwaitableResult { get; set; }
        public bool IsGenericTyped { get; set; }
        public bool IsInitOnlyProperty { get; set; }

        public readonly PropertyMetadata Build()
            => new()
            {
                GloballyQualifiedTypeName = GloballyQualifiedTypeName,
                IsChildContent = IsChildContent,
                IsDelegateSignature = IsDelegateSignature,
                IsEventCallback = IsEventCallback,
                IsDelegateWithAwaitableResult = IsDelegateWithAwaitableResult,
                IsGenericTyped = IsGenericTyped,
                IsInitOnlyProperty = IsInitOnlyProperty,
            };
    }
}
