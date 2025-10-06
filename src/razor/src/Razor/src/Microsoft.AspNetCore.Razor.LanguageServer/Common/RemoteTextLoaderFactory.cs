// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.CodeAnalysis;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Common;

internal abstract class RemoteTextLoaderFactory
{
    public abstract TextLoader Create(string filePath);
}
