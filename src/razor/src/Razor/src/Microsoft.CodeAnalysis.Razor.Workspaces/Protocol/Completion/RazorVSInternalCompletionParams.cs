// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Text.Json.Serialization;

namespace Roslyn.LanguageServer.Protocol;

/// <summary>
/// A replacement of the LSP protocol <see cref="CompletionParams"/> that ensures correct serialization between LSP servers.
/// </summary>
/// <remarks>
/// This is the same as the LSP protocol <see cref="CompletionParams"/> except that it strongly types the <see cref="Context"/> property as VSInternalCompletionContext,
/// because our custom message target gets handled by a JsonRpc connection set up by the editor, that has no Roslyn converters.
/// Without using this class, we lose VSInternalCompletionContext.InvokeKind property when calling Roslyn or HTML servers from Razor
/// which results in default value of "Invoked" to be used and can cause overly aggressive completion list.
///
/// See original CompletionParams here https://github.com/dotnet/roslyn/blob/98d41b80f6a192230c045a6576e2a283a407980b/src/LanguageServer/Protocol/Protocol/CompletionParams.cs
/// </remarks>
internal sealed class RazorVSInternalCompletionParams : TextDocumentPositionParams, IPartialResultParams<SumType<CompletionItem[], CompletionList>?>, IWorkDoneProgressOptions
{
    public RazorVSInternalCompletionParams()
    {
    }

    /// <summary>
    /// The completion context. This is only available if the client specifies the
    /// client capability <see cref="CompletionSetting.ContextSupport"/>.
    /// </summary>
    [JsonPropertyName("context")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public VSInternalCompletionContext? Context { get; set; }

    /// <inheritdoc/>
    [JsonPropertyName(Methods.PartialResultTokenName)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IProgress<SumType<CompletionItem[], CompletionList>?>? PartialResultToken { get; set; }

    /// <inheritdoc/>
    [JsonPropertyName("workDoneProgress")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool WorkDoneProgress { get; init; }
}
