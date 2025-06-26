// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

internal interface IDesignTimeCodeGenerator
{
    Task<RazorCodeDocument> GenerateDesignTimeOutputAsync(CancellationToken cancellationToken);
}
