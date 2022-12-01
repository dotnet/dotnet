// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Common;

public abstract class RemoteTextLoaderFactory
{
    internal abstract TextLoader Create(string filePath);
}
