// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Newtonsoft.Json.Linq;

namespace Microsoft.AspNetCore.Razor.LanguageServer.CodeActions;

internal interface IRazorCodeActionResolver : ICodeActionResolver
{
    Task<WorkspaceEdit?> ResolveAsync(JObject data, CancellationToken cancellationToken);
}
