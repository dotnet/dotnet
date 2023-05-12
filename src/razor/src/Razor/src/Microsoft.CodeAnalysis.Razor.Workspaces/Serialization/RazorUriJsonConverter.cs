// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.CodeAnalysis.Razor.Workspaces.Serialization;

/// <summary>
/// To workaround https://github.com/JamesNK/Newtonsoft.Json/issues/2128 we need to provide a Uri converter.
/// The LSP client has one, but it is not hooked up automatically and doesn't implement CanConvert, but we can
/// just do that on their behalf.
/// </summary>
internal class RazorUriJsonConverter : DocumentUriConverter
{
    public static readonly RazorUriJsonConverter Instance = new RazorUriJsonConverter();

    private RazorUriJsonConverter()
    {
    }

    public override bool CanConvert(Type objectType)
    {
        return typeof(Uri).IsAssignableFrom(objectType);
    }
}
