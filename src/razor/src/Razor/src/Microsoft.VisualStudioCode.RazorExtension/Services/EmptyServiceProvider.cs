// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.ExternalAccess.Razor;

namespace Microsoft.VisualStudioCode.RazorExtension.Services;

internal class EmptyServiceProvider : IRazorDocumentServiceProvider
{
    public static readonly EmptyServiceProvider Instance = new();

    public bool CanApplyChange => false;

    public bool SupportDiagnostics => false;

    public TService? GetService<TService>() where TService : class
    {
        return null;
    }
}
