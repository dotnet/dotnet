// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.AspNetCore.Razor.ProjectSystem;

internal interface IProjectEngineFactoryProvider
{
    IProjectEngineFactory GetFactory(RazorConfiguration configuration);
}
