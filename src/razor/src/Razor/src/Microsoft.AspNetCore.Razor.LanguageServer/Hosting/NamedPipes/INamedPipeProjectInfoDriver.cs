// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Hosting.NamedPipes;

internal interface INamedPipeProjectInfoDriver : IRazorProjectInfoDriver
{
    Task CreateNamedPipeAsync(string name, CancellationToken cancellationToken);
}
