// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Protocol;

/// <summary>
/// A seralizable pairing of <see cref="TextDocumentIdentifier"/> and a version. This
/// should be used over <see cref="VersionedTextDocumentIdentifier"/> because when serializing
/// and deserializing that class, if the <see cref="TextDocumentIdentifier"/> is a <see cref="VSTextDocumentIdentifier"/>
/// it will lose the project context information.
/// </summary>
internal record class TextDocumentIdentifierAndVersion(TextDocumentIdentifier TextDocumentIdentifier, int Version);
