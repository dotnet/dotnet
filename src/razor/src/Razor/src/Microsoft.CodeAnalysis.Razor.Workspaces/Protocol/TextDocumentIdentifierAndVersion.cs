// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Text.Json.Serialization;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.CodeAnalysis.Razor.Protocol;

/// <summary>
/// A serializable pairing of <see cref="TextDocumentIdentifier"/> and a version. This
/// should be used over <see cref="VersionedTextDocumentIdentifier"/> because when serializing
/// and deserializing that class, if the <see cref="TextDocumentIdentifier"/> is a <see cref="VSTextDocumentIdentifier"/>
/// it will lose the project context information.
/// </summary>
internal record class TextDocumentIdentifierAndVersion(
    [property:JsonPropertyName("textDocumentIdentifier")] TextDocumentIdentifier TextDocumentIdentifier,
    [property:JsonPropertyName("version")] int Version);
