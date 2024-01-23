// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Linq;
using Microsoft.AspNetCore.Razor.ProjectEngineHost;
using Microsoft.CodeAnalysis.Razor;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Common;

internal static class MefProjectEngineFactories
{
    // The actual factories are defined in the ProjectEngineHost project, but we need to convert them
    // to Lazy<,> to be MEF compatible.
    public static readonly Lazy<IProjectEngineFactory, ICustomProjectEngineFactoryMetadata>[] Factories =
        ProjectEngineFactories.Factories.Select(f => new Lazy<IProjectEngineFactory, ICustomProjectEngineFactoryMetadata>(() => f.Item1.Value, f.Item2)).ToArray();
}
