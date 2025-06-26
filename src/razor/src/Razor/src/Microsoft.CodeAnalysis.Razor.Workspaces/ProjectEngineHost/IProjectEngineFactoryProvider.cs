// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.CodeAnalysis.Razor.ProjectEngineHost;

internal interface IProjectEngineFactoryProvider
{
    IProjectEngineFactory GetFactory(RazorConfiguration configuration);
}
