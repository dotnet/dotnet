// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.Razor.Workspaces.Protocol;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.CodeAnalysis.Razor.DocumentMapping;

/// <summary>
/// Represents a position in a document. If <see cref="LanguageKind"/> is Razor then the position will be
/// in the host document, otherwise it will be in the corresponding generated document.
/// </summary>
internal record DocumentPositionInfo(RazorLanguageKind LanguageKind, Position Position, int HostDocumentIndex);
