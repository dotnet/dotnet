using System.Runtime.Serialization;
using Microsoft.CodeAnalysis.Razor.Formatting;
using Microsoft.CodeAnalysis.Razor.Settings;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.CodeAnalysis.Razor.Remote;

[DataContract]
internal readonly record struct RemoteAutoInsertOptions
{
    [DataMember(Order = 0)]
    public bool EnableAutoClosingTags { get; init; } = true;

    [DataMember(Order = 1)]
    public bool FormatOnType { get; init; } = true;

    [DataMember(Order = 2)]
    public RazorFormattingOptions FormattingOptions { get; init; } = new RazorFormattingOptions()
    {
        InsertSpaces = true,
        TabSize = 4
    };

    public RemoteAutoInsertOptions()
    {
    }

    public static RemoteAutoInsertOptions From(ClientSettings clientSettings, FormattingOptions formattingOptions)
        => new()
        {
            EnableAutoClosingTags = clientSettings.AdvancedSettings.AutoClosingTags,
            FormatOnType = clientSettings.AdvancedSettings.FormatOnType,
            FormattingOptions = RazorFormattingOptions.From(formattingOptions, codeBlockBraceOnNextLine: false)
        };
}
