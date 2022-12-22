// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor.HtmlCSharp;

internal record CompletionRequestContext(Uri HostDocumentUri, Uri ProjectedDocumentUri, LanguageServerKind LanguageServerKind);
