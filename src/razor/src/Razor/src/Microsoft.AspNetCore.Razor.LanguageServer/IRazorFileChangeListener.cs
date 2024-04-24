// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal interface IRazorFileChangeListener
{
    Task RazorFileChangedAsync(string filePath, RazorFileChangeKind kind, CancellationToken cancellationToken);
}
