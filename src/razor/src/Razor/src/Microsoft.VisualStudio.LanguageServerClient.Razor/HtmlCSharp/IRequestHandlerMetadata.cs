// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace Microsoft.VisualStudio.LanguageServerClient.Razor.HtmlCSharp;

internal interface IRequestHandlerMetadata
{
    /// <summary>
    /// Name of the LSP method to handle.
    /// </summary>
    string MethodName { get; }
}
