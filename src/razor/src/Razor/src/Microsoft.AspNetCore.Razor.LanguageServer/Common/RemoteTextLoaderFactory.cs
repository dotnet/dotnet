// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Common;

internal abstract class RemoteTextLoaderFactory
{
    public abstract TextLoader Create(string filePath);
}
