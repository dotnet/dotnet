// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.CodeAnalysis.ExternalAccess.Razor;

namespace Microsoft.VisualStudioCode.RazorExtension.Services;

internal sealed class EmptyServiceProvider : IRazorDocumentServiceProvider
{
    public static readonly EmptyServiceProvider Instance = new();

    public bool CanApplyChange => false;

    public bool SupportDiagnostics => true;

    public TService? GetService<TService>() where TService : class
    {
        return null;
    }
}
