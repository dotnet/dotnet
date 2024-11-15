// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Hosting.NamedPipes;

internal interface INamedPipeProjectInfoDriver : IRazorProjectInfoDriver
{
    Task CreateNamedPipeAsync(string name, CancellationToken cancellationToken);
}
