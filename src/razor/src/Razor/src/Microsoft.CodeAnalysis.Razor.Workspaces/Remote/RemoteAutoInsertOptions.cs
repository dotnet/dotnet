// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Runtime.Serialization;
using Microsoft.CodeAnalysis.Razor.Formatting;
using Microsoft.CodeAnalysis.Razor.Settings;

namespace Microsoft.CodeAnalysis.Razor.Remote;

[DataContract]
internal readonly record struct RemoteAutoInsertOptions
{
    [DataMember(Order = 0)]
    public bool EnableAutoClosingTags { get; init; } = true;

    [DataMember(Order = 1)]
    public bool FormatOnType { get; init; } = true;

    [DataMember(Order = 2)]
    public RazorFormattingOptions FormattingOptions { get; init; } = new();

    public RemoteAutoInsertOptions()
    {
    }

    public static RemoteAutoInsertOptions From(ClientSettings clientSettings, RazorFormattingOptions formattingOptions)
        => new()
        {
            EnableAutoClosingTags = clientSettings.AdvancedSettings.AutoClosingTags,
            FormatOnType = clientSettings.AdvancedSettings.FormatOnType,
            FormattingOptions = formattingOptions
        };
}
