// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace Microsoft.CodeAnalysis.Razor.Protocol.InlayHints;

internal record class InlayHintDataWrapper(TextDocumentIdentifier TextDocument, object? OriginalData, Position OriginalPosition);
