// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal abstract class DocumentContextFactory
{
    public abstract Task<DocumentContext?> TryCreateAsync(Uri documentUri, CancellationToken cancellationToken);
}
