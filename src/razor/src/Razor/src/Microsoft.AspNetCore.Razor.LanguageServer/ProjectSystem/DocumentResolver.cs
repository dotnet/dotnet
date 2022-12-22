// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;

internal abstract class DocumentResolver
{
    public abstract bool TryResolveDocument(string documentFilePath, [NotNullWhen(true)] out DocumentSnapshot? document);
}
