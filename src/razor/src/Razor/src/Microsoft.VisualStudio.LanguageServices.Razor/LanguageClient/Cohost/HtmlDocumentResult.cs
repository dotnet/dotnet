// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.VisualStudio.Text;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

internal record HtmlDocumentResult(Uri Uri, ITextBuffer Buffer);
