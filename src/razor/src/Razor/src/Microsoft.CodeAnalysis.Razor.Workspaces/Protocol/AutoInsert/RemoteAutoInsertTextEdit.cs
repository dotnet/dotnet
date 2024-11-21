// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Runtime.Serialization;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using static Microsoft.VisualStudio.LanguageServer.Protocol.VsLspExtensions;
using static Roslyn.LanguageServer.Protocol.RoslynLspExtensions;
using RoslynInsertTextFormat = Roslyn.LanguageServer.Protocol.InsertTextFormat;

namespace Microsoft.CodeAnalysis.Razor.Protocol.AutoInsert;

[DataContract]
internal readonly record struct RemoteAutoInsertTextEdit(
    [property: DataMember(Order = 0)] LinePositionSpan LinePositionSpan,
    [property: DataMember(Order = 1)] string NewText,
    [property: DataMember(Order = 2)] RoslynInsertTextFormat InsertTextFormat)
{
    public static RemoteAutoInsertTextEdit FromLspInsertTextEdit(VSInternalDocumentOnAutoInsertResponseItem edit)
        => new(
            edit.TextEdit.Range.ToLinePositionSpan(),
            edit.TextEdit.NewText,
            (RoslynInsertTextFormat)edit.TextEditFormat);

    public static VSInternalDocumentOnAutoInsertResponseItem ToLspInsertTextEdit(RemoteAutoInsertTextEdit edit)
        => new()
        {
            TextEdit = VsLspFactory.CreateTextEdit(edit.LinePositionSpan, edit.NewText),
            TextEditFormat = (InsertTextFormat)edit.InsertTextFormat,
        };

    public override string ToString()
    {
        return $"({LinePositionSpan}), '{NewText}', {InsertTextFormat}";
    }
}
